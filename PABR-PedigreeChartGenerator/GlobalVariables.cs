using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PABR_PedigreeChartGenerator
{
    public static class LoginDetails
    {
        public static string? PaccessToken = "";
        public static string? PuserID = "";
        public static string? PuserEmail = "";
        public static string? PuserFName = "";
        public static string? PuserLName = "";

        public static string? accessToken
        {
            get
            {
                return PaccessToken;
            }
            set
            {
                PaccessToken = value;
            }
        }
        public static string? userID
        {
            get
            {
                return PuserID;
            }
            set
            {
                PuserID = value;
            }
        }
        public static string? userEmail
        {
            get
            {
                return PuserEmail;
            }
            set
            {
                PuserEmail = value;
            }
        }
        public static string? userFName
        {
            get
            {
                return PuserFName;
            }
            set
            {
                PuserFName = value;
            }
        }
        public static string? userLName
        {
            get
            {
                return PuserLName;
            }
            set
            {
                PuserLName = value;
            }
        }
        public static void ClearProperties()
        {
            // Loop through the static properties of the model
            foreach (var field in typeof(LoginDetails).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                // Set the value of the static property to its default value
                field.SetValue(null, default);
            }
        }
    }

    public static class CurSelectedDog
    {
        public static string? PUID = "";
        public static string? PDogName = "";
        public static string? PGender = "";
        public static string? PBreed = "";
        public static string? PColor = "";
        public static string? POwnerName = "";
        public static string? PPABRno = "";
        public static string? PRegistryNo = "";
        public static string? PDateAdded = "";
        public static string? PPicURL = "";

        public static string? UID
        {
            get
            {
                return PUID;
            }
            set
            {
                PUID = value;
            }
        }
        public static string? DogName
        {
            get
            {
                return PDogName;
            }
            set
            {
                PDogName = value;
            }
        }
        public static string? Gender
        {
            get
            {
                return PGender;
            }
            set
            {
                PGender = value;
            }
        }
        public static string? Breed
        {
            get
            {
                return PBreed;
            }
            set
            {
                PBreed = value;
            }
        }
        public static string? Color
        {
            get
            {
                return PColor;
            }
            set
            {
                PColor = value;
            }
        }
        public static string? OwnerName
        {
            get
            {
                return POwnerName;
            }
            set
            {
                POwnerName = value;
            }
        }
        public static string? PABRno
        {
            get
            {
                return PPABRno;
            }
            set
            {
                PPABRno = value;
            }
        }
        public static string? RegistryNo
        {
            get
            {
                return PRegistryNo;
            }
            set
            {
                PRegistryNo = value;
            }
        }
        public static string? DateAdded
        {
            get
            {
                return PDateAdded;
            }
            set
            {
                PDateAdded = value;
            }
        }
        public static string? PicURL
        {
            get
            {
                return PPicURL;
            }
            set
            {
                PPicURL = value;
            }
        }

        public static void ClearProperties()
        {
            // Loop through the static properties of the model
            foreach (var field in typeof(CurSelectedDog).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                // Set the value of the static property to its default value
                field.SetValue(null, default);
            }
        }
    }

    public static class CurSelectedPCField
    {
        public static string? PPCField = "";

        public static string? PCField
        {
            get
            {
                return PPCField;
            }
            set
            {
                PPCField = value;
            }
        }
    }
}
