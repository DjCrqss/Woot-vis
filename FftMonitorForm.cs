using NAudio.CoreAudioApi;
using NAudio.Wave;
using ScottPlot;
using System.Diagnostics;
using Wooting;

namespace AudioMonitor;

public partial class FftMonitorForm : Form
{
    readonly double[] AudioValues;
    private bool useRGB = false;

    readonly WasapiCapture AudioDevice;
    readonly double[] FftValues;
    private double[][] oldValues;

    // Wooting RGB values
    private int count;
    private RGBDeviceInfo[] infos;

    int highestRange = 1800; // highest for 20khz is 4097;
    double highestPower = 0.008;





    public FftMonitorForm(WasapiCapture audioDevice)
    {
        InitializeComponent();
        AudioDevice = audioDevice;
        WaveFormat fmt = audioDevice.WaveFormat;

        AudioValues = new double[fmt.SampleRate / 10];
        double[] paddedAudio = FftSharp.Pad.ZeroPad(AudioValues);
        double[] fftMag = FftSharp.Transform.FFTpower(paddedAudio);
        FftValues = new double[fftMag.Length];
        double fftPeriod = FftSharp.Transform.FFTfreqPeriod(fmt.SampleRate, fftMag.Length);

        formsPlot1.Plot.Palette = ScottPlot.Palette.OneHalfDark;
        formsPlot1.Plot.AddSignal(FftValues, 1.0 / fftPeriod);
        formsPlot1.Plot.AddHorizontalLine(highestPower);
        formsPlot1.Plot.AddVerticalLine(highestRange / (1 / fftPeriod));
        formsPlot1.Plot.YLabel("Spectral Power");
        formsPlot1.Plot.XLabel("Frequency (kHz)");
        formsPlot1.Plot.Title($"{fmt.Encoding} ({fmt.BitsPerSample}-bit) {fmt.SampleRate} KHz");
        formsPlot1.Plot.SetAxisLimits(0, 6000, 0, .005);

        formsPlot1.Plot.Style(ScottPlot.Style.Gray1);
        var bnColor = System.Drawing.ColorTranslator.FromHtml("#121212");
        formsPlot1.Plot.Style(figureBackground: bnColor, dataBackground: bnColor);


        formsPlot1.Refresh();

        AudioDevice.DataAvailable += WaveIn_DataAvailable;
        AudioDevice.StartRecording();

        FormClosed += FftMonitorForm_FormClosed;

        // Try RGB
        useRGB = runRGB();
    }

    private bool runRGB()
    {
        // Set up RGB
        if (!RGBControl.IsConnected()) return false;
        useRGB = true;
        count = RGBControl.GetDeviceCount();
        infos = new RGBDeviceInfo[count];
        for (byte i = 0; i < count; i++)
        {
            RGBControl.SetControlDevice(i);
            var device = RGBControl.GetDeviceInfo();
            infos[i] = device;
        }
        oldValues = new double[count][];
        for (byte idx = 0; idx < count; idx++)
        {
            RGBControl.SetControlDevice(idx);
            var device = infos[idx];
            KeyColour[,] keys = new KeyColour[RGBControl.MaxRGBRows, RGBControl.MaxRGBCols];
            // set up decay array.
            oldValues[idx] = new double[device.MaxColumns];
            for (byte i = 0; i < device.MaxColumns; i++)
            {
                for (byte j = 0; j < device.MaxRows; j++)
                {
                    keys[j, i] = new KeyColour(20, 20, 20);
                }
            }
            RGBControl.SetFull(keys);
            RGBControl.UpdateKeyboard();
        }
        return true;
    }

    private void FftMonitorForm_FormClosed(object? sender, FormClosedEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine($"Closing audio device: {AudioDevice}");
        AudioDevice.StopRecording();
        AudioDevice.Dispose();

        if (useRGB) {
            useRGB = false;
            for (byte idx = 0; idx < count; idx++)
            {
                RGBControl.SetControlDevice(idx);
                RGBControl.ResetRGB();
            }
        }
    }

    private void WaveIn_DataAvailable(object? sender, WaveInEventArgs e)
    {
        int bytesPerSamplePerChannel = AudioDevice.WaveFormat.BitsPerSample / 8;
        int bytesPerSample = bytesPerSamplePerChannel * AudioDevice.WaveFormat.Channels;
        int bufferSampleCount = e.Buffer.Length / bytesPerSample;

        if (bufferSampleCount >= AudioValues.Length)
        {
            bufferSampleCount = AudioValues.Length;
        }

        if (bytesPerSamplePerChannel == 2 && AudioDevice.WaveFormat.Encoding == WaveFormatEncoding.Pcm)
        {
            for (int i = 0; i < bufferSampleCount; i++)
                AudioValues[i] = BitConverter.ToInt16(e.Buffer, i * bytesPerSample);
        }
        else if (bytesPerSamplePerChannel == 4 && AudioDevice.WaveFormat.Encoding == WaveFormatEncoding.Pcm)
        {
            for (int i = 0; i < bufferSampleCount; i++)
                AudioValues[i] = BitConverter.ToInt32(e.Buffer, i * bytesPerSample);
        }
        else if (bytesPerSamplePerChannel == 4 && AudioDevice.WaveFormat.Encoding == WaveFormatEncoding.IeeeFloat)
        {
            for (int i = 0; i < bufferSampleCount; i++)
                AudioValues[i] = BitConverter.ToSingle(e.Buffer, i * bytesPerSample);
        }
        else
        {
            throw new NotSupportedException(AudioDevice.WaveFormat.ToString());
        }
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
        double[] paddedAudio = FftSharp.Pad.ZeroPad(AudioValues);
        double[] fftMag = FftSharp.Transform.FFTmagnitude(paddedAudio);

        //label1.Text = $"Length: {fftMag.Length}";
        Array.Copy(fftMag, FftValues, fftMag.Length);


        if (useRGB)
        {
            // run RGB
            for (byte idx = 0; idx < count; idx++)
            {
                RGBControl.SetControlDevice(idx);
                var device = infos[idx];

                int maxCols = device.MaxColumns;
                int maxRows = device.MaxRows;

                

                double[] averagedArray = SplitAndAverage(fftMag, Math.Min(highestRange, fftMag.Length), maxCols);

                double[] normalisedArray = NormaliseValues(averagedArray);

                double[] scaledArray = ScaleValues(normalisedArray, highestPower, maxRows);

                // update decay values
                for (int i = 0; i < maxCols; i++) {
                    if (oldValues[idx][i] <= scaledArray[i])
                    {
                        oldValues[idx][i] = scaledArray[i];
                    } else if (oldValues[idx][i] > 0){
                        oldValues[idx][i] -= 1;
                    }
                }


                //label1.Text = $"Length: {RGBControl.MaxRGBCols} Last: {scaledArray[maxCols-1]}";

                KeyColour[,] keys = new KeyColour[RGBControl.MaxRGBRows, RGBControl.MaxRGBCols];
                int range = 255 / maxCols;
                for (byte i = 0; i < maxCols; i++)
                {
                    byte color = Convert.ToByte(range * i);
                    
                    for (byte j = 0; j < maxRows; j++)
                    {
                        if (j >= maxRows - Math.Max(scaledArray[i], oldValues[idx][i])) {
                            keys[j, i] = new KeyColour(color, Convert.ToByte(255 - color), 20);
                        } else {
                            keys[j, i] = new KeyColour(20, 20, 20);
                        }
                       
                    }
                }
                RGBControl.SetFull(keys);
                RGBControl.UpdateKeyboard();
            }

           
        }


        // request a redraw using a non-blocking render queue
        formsPlot1.RefreshRequest();
    }

    private double[] SplitAndAverage(double[] fftMag, int splitRange, int maxCols) {
        double[] resultArray = new double[maxCols];
        int range = splitRange / maxCols;

       

        for (int i = 0; i < maxCols; i++) {
            double highest = 0;
            double sum = 0;
            for (int j = range * i; j < (range * i) + range; j++) {
                if (fftMag[j] > highest) { highest = fftMag[j]; }
                sum += fftMag[j];
            }
            resultArray[i] = (3 * highest + (sum/range))/4;

            //for (int j = range * i; j < (range * i) + range; j++) {
            //    sum += fftMag[j];
            //}
            //resultArray[i] = sum/range;
        }

        return resultArray;
    }
    private double[] ScaleValues(double[] inputArray, double highestPower, int maxRows)
    {
        double scaleFactor = maxRows / highestPower;

        // Scale each value in the array
        for (int i = 0; i < inputArray.Length; i++)
        {
            inputArray[i] = (int)(inputArray[i] * scaleFactor);
            if (inputArray[i] > maxRows) inputArray[i] = maxRows;
        }

        return inputArray;
    }
    private double[] NormaliseValues(double[] inputArray) {
        for (int x = 0; x < inputArray.Length; x++)
        {
            inputArray[x] *= (2 * Math.Log10(3 * x + 1) + 1);
        }
        return inputArray;
    }
}
