using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalGenerator.Logic
{
    public enum SignalType
    {
        Sine,
        Saw,
        Rectangle
    }

    public class SignalGenerator
    {
        private SignalType _signalType;


        private float _frequency = 0f;
        private float _phase = 0f;
        private float _amplitude = 0f;
        private float _offset = 0f;
        private float _invert = 0f;
        private long startTime = Stopwatch.GetTimestamp();
        private long ticksPerSecond = Stopwatch.Frequency;

        public float Frequency { get { return _frequency; } set { _frequency = value; } }
        public float Phase { get { return _phase; } set { _phase = value; } }
        public float Amplitude { get { return _amplitude; } set { _amplitude = value; } }
        public float Offset { get { return _offset; } set { _offset = value; } }
        public float Invert { get { return _invert; } set { _invert = value; } }

        public SignalGenerator(SignalType type)
        {
            _signalType = type;
        }

        private float GetValue(float time)
        {
            float value = 0f;
            float t = _frequency * time + _phase;
            switch (_signalType)
            { // http://en.wikipedia.org/wiki/Waveform
                case SignalType.Sine: // sin( 2 * pi * t )
                    value = (float)Math.Sin(2f * Math.PI * t);
                    break;
                case SignalType.Rectangle: // sign( sin( 2 * pi * t ) )
                    value = Math.Sign(Math.Sin(2f * Math.PI * t));
                    break;
                case SignalType.Saw:
                    // 2 * ( t/a - floor( t/a + 1/2 ) )
                    value = 2f * (t - (float)Math.Floor(t + 0.5f));
                    break;
            }

            return (_invert * _amplitude * value + _offset);
        }

        public float GetValue()
        {
            float time = (float)(Stopwatch.GetTimestamp() - startTime)
                            / ticksPerSecond;
            return GetValue(time);
        }

        public void Reset()
        {
            startTime = Stopwatch.GetTimestamp();
        }

    }
}
