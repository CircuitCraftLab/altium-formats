using System.Globalization;

namespace CircuitCraftLab.AltiumFormats;

public static class EnumExtensions {
    public static T WithFlag<T>(this T enumerable, T flag, bool value = true) where T : Enum {
        var intEnum = Convert.ToInt32(enumerable, CultureInfo.InvariantCulture);
        var intFlag = Convert.ToInt32(flag, CultureInfo.InvariantCulture);
        if (value) {
            return (T) Enum.ToObject(typeof(T), intEnum | intFlag);
        } else {
            return (T) Enum.ToObject(typeof(T), intEnum & ~intFlag);
        }
    }

    public static void SetFlag<T>(ref this T enumerable, T flag, bool value = true) where T : struct, Enum =>
        enumerable = WithFlag(enumerable, flag, value);

    public static void ClearFlag<T>(ref this T enumerable, T flag) where T : struct, Enum =>
        SetFlag(ref enumerable, flag, false);
}
