﻿namespace System
{
    using System.Linq;
    using PassKitHelper;

    public static class EnumExtensions
    {
        public static string ToPassKitString(this BarcodeFormat value)
        {
            return value switch
            {
                BarcodeFormat.QR => "PKBarcodeFormatQR",
                BarcodeFormat.Pdf417 => "PKBarcodeFormatPDF417",
                BarcodeFormat.Aztec => "PKBarcodeFormatAztec",
                BarcodeFormat.Code128 => "PKBarcodeFormatCode128",
                _ => throw new Exception("Unknown BarcodeFormat value: " + value),
            };
        }

        public static string ToPassKitString(this TransitType value)
        {
            return value switch
            {
                TransitType.Air => "PKTransitTypeAir",
                TransitType.Boat => "PKTransitTypeBoat",
                TransitType.Bus => "PKTransitTypeBus",
                TransitType.Generic => "PKTransitTypeGeneric",
                TransitType.Train => "PKTransitTypeTrain",
                _ => throw new Exception("Unknown TransitType value: " + value),
            };
        }

        public static string ToPassKitString(this DateStyle value)
        {
            return value switch
            {
                DateStyle.None => "PKDateStyleNone",
                DateStyle.Short => "PKDateStyleShort",
                DateStyle.Medium => "PKDateStyleMedium",
                DateStyle.Long => "PKDateStyleLong",
                DateStyle.Full => "PKDateStyleFull",
                _ => throw new Exception("Unknown DateStyle value: " + value),
            };
        }

        public static string ToPassKitString(this NumberStyle value)
        {
            return value switch
            {
                NumberStyle.Decimal => "PKNumberStyleDecimal",
                NumberStyle.Percent => "PKNumberStylePercent",
                NumberStyle.Scientific => "PKNumberStyleScientific",
                NumberStyle.SpellOut => "PKNumberStyleSpellOut",
                _ => throw new Exception("Unknown NumberStyle value: " + value),
            };
        }

        public static string ToPassKitString(this TextAlignment value)
        {
            return value switch
            {
                TextAlignment.Left => "PKTextAlignmentLeft",
                TextAlignment.Center => "PKTextAlignmentCenter",
                TextAlignment.Right => "PKTextAlignmentRight",
                TextAlignment.Natural => "PKTextAlignmentNatural",
                _ => throw new Exception("Unknown TextAlignment value: " + value),
            };
        }

        public static string[] ToPassKitString(this DataDetectorType values)
        {
            if (values == DataDetectorType.None)
            {
                return Array.Empty<string>();
            }

            return values
                .ToString()
                .Split(',')
                .Select(x => "PKDataDetectorType" + x.Trim())
                .ToArray();
        }
    }
}
