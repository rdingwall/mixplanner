using System;
using System.Collections.Generic;
using System.Globalization;

namespace MixPlanner.DomainModel
{
    public struct HarmonicKey : IEquatable<HarmonicKey>, IComparable<HarmonicKey>, IComparable
    {
        static readonly Random random = new Random();
        
        // e.g. 12A
        private readonly ushort hexValue;

        const ushort HexValueIfUnknown = 0;
        const int PitchIfUnknown = 0;

        private readonly Scale scale;
        private readonly int pitch;

        static HarmonicKey()
        {
            AllKeys = new[]
                          {
                              Key1A,
                              Key1B,
                              Key2A,
                              Key2B,
                              Key3A,
                              Key3B,
                              Key4A,
                              Key4B,
                              Key5A,
                              Key5B,
                              Key6A,
                              Key6B,
                              Key7A,
                              Key7B,
                              Key8A,
                              Key8B,
                              Key9A,
                              Key9B,
                              Key10A,
                              Key10B,
                              Key11A,
                              Key11B,
                              Key12A,
                              Key12B,
                              Unknown
                          };
        }

        public HarmonicKey(int pitch, Scale scale) : this (String.Format("{0}{1:X}", pitch, (int)scale))
        {
        }

        public HarmonicKey(string key)
        {
            if (key == null) throw new ArgumentNullException("key");

            hexValue = ushort.Parse(key, NumberStyles.HexNumber);

            scale = ExtractScale(hexValue);
            pitch = ExtractPitch(hexValue);
        }

        HarmonicKey(ushort hexValue, Scale scale, int pitch)
        {
            this.hexValue = hexValue;
            this.scale = scale;
            this.pitch = pitch;
        }

        static int ExtractPitch(ushort key)
        {
            // Isolate the pitch e.g. 12A -> 12
            var hexPitchAsDec = key >> 4;
            var pitch = Convert.ToInt32(hexPitchAsDec.ToString("X"));

            ThrowIfInvalidPitch(pitch);

            return pitch;
        }

        static void ThrowIfInvalidPitch(int pitch)
        {
            if (!(pitch <= 12))
                throw new InvalidPitchException(String.Format("{0} is not a valid pitch (must be 0-12).", pitch));
        }

        static Scale ExtractScale(ushort key)
        {
            // Isolate the scale e.g. 12B -> B
            var scale = key & 0xF;

            ThrowIfInvalidScale(scale);

            return (Scale)scale;
        }

        static void ThrowIfInvalidScale(int scale)
        {
            if (!(scale == (ushort)Scale.Major || scale == (ushort)Scale.Minor))
                throw new InvalidScaleException("Invalid scale (must be A or B).");
        }

        public bool HasSamePitchAs(HarmonicKey other)
        {
            return Pitch.Equals(other.Pitch);
        }

        public bool HasSameScaleAs(HarmonicKey other)
        {
            return Scale.Equals(other.Scale);
        }

        public bool IsMajor()
        {
            return Scale == Scale.Major;
        }

        public bool IsMinor()
        {
            return Scale == Scale.Minor;
        }

        public Scale Scale
        {
            get { return scale; }
        }

        public int Pitch
        {
            get { return pitch; }
        }

        public bool IsUnknown
        {
            get { return hexValue == HexValueIfUnknown; }
        }

        public override string ToString()
        {
            if (IsUnknown)
                return "Unknown Key";

            return hexValue.ToString("X");
        }

        public static readonly HarmonicKey Key1A = new HarmonicKey("1A"); 
        public static readonly HarmonicKey Key2A = new HarmonicKey("2A"); 
        public static readonly HarmonicKey Key3A = new HarmonicKey("3A"); 
        public static readonly HarmonicKey Key4A = new HarmonicKey("4A"); 
        public static readonly HarmonicKey Key5A = new HarmonicKey("5A"); 
        public static readonly HarmonicKey Key6A = new HarmonicKey("6A"); 
        public static readonly HarmonicKey Key7A = new HarmonicKey("7A"); 
        public static readonly HarmonicKey Key8A = new HarmonicKey("8A"); 
        public static readonly HarmonicKey Key9A = new HarmonicKey("9A"); 
        public static readonly HarmonicKey Key10A = new HarmonicKey("10A");
        public static readonly HarmonicKey Key11A = new HarmonicKey("11A");
        public static readonly HarmonicKey Key12A = new HarmonicKey("12A");
                       
        public static readonly HarmonicKey Key1B = new HarmonicKey("1B"); 
        public static readonly HarmonicKey Key2B = new HarmonicKey("2B"); 
        public static readonly HarmonicKey Key3B = new HarmonicKey("3B"); 
        public static readonly HarmonicKey Key4B = new HarmonicKey("4B"); 
        public static readonly HarmonicKey Key5B = new HarmonicKey("5B"); 
        public static readonly HarmonicKey Key6B = new HarmonicKey("6B"); 
        public static readonly HarmonicKey Key7B = new HarmonicKey("7B"); 
        public static readonly HarmonicKey Key8B = new HarmonicKey("8B"); 
        public static readonly HarmonicKey Key9B = new HarmonicKey("9B"); 
        public static readonly HarmonicKey Key10B = new HarmonicKey("10B");
        public static readonly HarmonicKey Key11B = new HarmonicKey("11B");
        public static readonly HarmonicKey Key12B = new HarmonicKey("12B");

        public static readonly HarmonicKey Unknown =
            new HarmonicKey(hexValue: HexValueIfUnknown,
                            scale: Scale.Unknown,
                            pitch: PitchIfUnknown);

        public static HarmonicKey RandomKey()
        {
            var pitch = random.Next(1, 13);
            var scale = random.Next(0, 2) == 1 ? Scale.Major : Scale.Minor;
            return new HarmonicKey(pitch, scale);
        }

        // Advances n number of positions around the Camelot wheel.
        public HarmonicKey Advance(int value)
        {
            if (IsUnknown)
                return this;

            var newPitch = (Pitch + value) % 12;

            if (newPitch == 0)
                newPitch = 12;

            if (newPitch < 0)
                newPitch += 12;

            return new HarmonicKey(newPitch, Scale);
        }

        public HarmonicKey ToMinor()
        {
            if (IsUnknown)
                return this;

            return new HarmonicKey(Pitch, Scale.Minor);
        }

        public HarmonicKey ToMajor()
        {
            if (IsUnknown)
                return this;

            return new HarmonicKey(Pitch, Scale.Major);
        }

        public static bool TryParse(string str, out HarmonicKey key)
        {
            try
            {
                key = new HarmonicKey(str);
                return true;
            }
            catch
            {
                key = default(HarmonicKey);
                return false;
            }
        }

        public bool Equals(HarmonicKey other)
        {
            return hexValue == other.hexValue;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is HarmonicKey && Equals((HarmonicKey) obj);
        }

        public override int GetHashCode()
        {
            return hexValue.GetHashCode();
        }

        public int CompareTo(HarmonicKey other)
        {
            return hexValue - other.hexValue;
        }

        public int CompareTo(object obj)
        {
            return CompareTo((HarmonicKey)obj);
        }

        public static bool operator ==(HarmonicKey left, HarmonicKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(HarmonicKey left, HarmonicKey right)
        {
            return !left.Equals(right);
        }

        public static IEnumerable<HarmonicKey> AllKeys { get; private set; }
    }
}