using System;

namespace ImageHosting.Storage.Features.Images.Models;

public enum SizeValues : short
{
    Size100 = 100,
    Size250 = 250,
    Size500 = 500
}

public readonly record struct SizeParam(SizeValues Value) : IParsable<SizeParam>
{
    public static SizeParam Parse(string s, IFormatProvider? provider)
    {
        if (!TryParse(s, provider, out var result))
        {
            throw new ArgumentException("Invalid value for size param.", nameof(s));
        }

        return result;
    }

    public static bool TryParse(string? s, IFormatProvider? provider, out SizeParam result)
    {
        var size = Convert.ToInt16(s);

        switch (size)
        {
            case (short)SizeValues.Size100:
                result = new SizeParam(SizeValues.Size100);
                return true;

            case (short)SizeValues.Size250:
                result = new SizeParam(SizeValues.Size250);
                return true;

            case (short)SizeValues.Size500:
                result = new SizeParam(SizeValues.Size500);
                return true;

            default:
                result = default;
                return false;
        }
    }
}