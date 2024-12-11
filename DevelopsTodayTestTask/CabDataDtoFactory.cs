using System.Globalization;

namespace DevelopsTodayTestTask;

public sealed class CabDataDtoFactory
{
    public static CabDataDto Create(
        string tpepPickupDatetimeValue,
        string tpepDropoffDatetimeValue,
        string passengerCountValue,
        string tripDistanceValue,
        string storeAndFwdFlagValue,
        string puLocationIdValue,
        string doLocationIdValue,
        string fareAmountValue,
        string tipAmountValue)
    {
        var estTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

        if (!DateTime.TryParse(tpepPickupDatetimeValue, CultureInfo.InvariantCulture, out var tpepPickupDatetimeEst))
        {
            throw new ArgumentException("Invalid tpep pickup datetime value.", nameof(tpepPickupDatetimeValue));
        }

        if (!DateTime.TryParse(tpepDropoffDatetimeValue, CultureInfo.InvariantCulture, out var tpepDropoffDatetimeEst))
        {
            throw new ArgumentException("Invalid tpep dropoff datetime value.", nameof(tpepDropoffDatetimeValue)); 
        }

        if (!int.TryParse(passengerCountValue, out var passengerCount))
        {
            throw new ArgumentException("Invalid passenger count.", nameof(passengerCount));
        }

        if (passengerCount <= 0)
        {
            throw new ArgumentException("Invalid passenger count.", nameof(passengerCount));
        }

        if (!double.TryParse(tripDistanceValue, out var tripDistance))
        {
            throw new ArgumentException("Invalid trip distance.", nameof(tripDistance));
        }

        if (tripDistance <= 0)
        {
            throw new ArgumentException("Invalid trip distance.", nameof(tripDistance));
        }
        
        var storeAndFwdFlag = TransformStoreAndFwdFlag(storeAndFwdFlagValue);

        if (!int.TryParse(puLocationIdValue, out var puLocationId))
        {
            throw new ArgumentException("Invalid pu location ID.", nameof(puLocationIdValue));
        }

        if (!int.TryParse(doLocationIdValue, out var doLocationId))
        {
            throw new ArgumentException("Invalid do location ID.", nameof(doLocationIdValue));
        }

        if (!decimal.TryParse(fareAmountValue, out var fareAmount))
        {
            throw new ArgumentException("Invalid fare amount.", nameof(fareAmount));
        }

        if (fareAmount <= 0)
        {
            throw new ArgumentException("Invalid fare amount.", nameof(fareAmount));
        }

        if (!decimal.TryParse(tipAmountValue, out var tipAmount))
        {
            throw new ArgumentException("Invalid tip amount.", nameof(tipAmount));
        }
        
        var tpepPickupDatetimeUtc = TimeZoneInfo.ConvertTimeToUtc(tpepPickupDatetimeEst, estTimeZoneInfo);
        var tpepDropoffDatetimeUtc = TimeZoneInfo.ConvertTimeToUtc(tpepDropoffDatetimeEst, estTimeZoneInfo);
        
        return new CabDataDto(
            tpepPickupDatetimeUtc,
            tpepDropoffDatetimeUtc,
            passengerCount,
            tripDistance,
            storeAndFwdFlag,
            puLocationId,
            doLocationId,
            fareAmount,
            tipAmount);
    }

    private static string TransformStoreAndFwdFlag(string storeAndFwdFlag)
    {
        if (storeAndFwdFlag == "N")
        {
            return "No";
        }

        if (storeAndFwdFlag == "Y")
        {
            return "Yes";
        }

        return storeAndFwdFlag.Trim();
    }
}
