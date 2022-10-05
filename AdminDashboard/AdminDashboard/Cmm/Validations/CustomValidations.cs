using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace DSELN.Cmm.Validations
{
    public class DateVaild : ValidationAttribute // 유효 날짜 여부 
    {
        private readonly string _comparisonProperty;

        public DateVaild(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        /*        public override bool IsValid(object value)
                {
                    if (value == null)
                    {
                        return true;
                    }
                    else
                    {
                        string date = value.ToString().Replace("-", "").Replace(".", "");
                        Console.WriteLine("birthdate : " + date);

                        return Cmm.Utils.Utils.IsDate(date);
                    }
                }*/

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = validationContext.ObjectInstance;
            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (property.GetValue(validationContext.ObjectInstance) == null)
            {
                return ValidationResult.Success;
            }
            else
            {
                //Console.WriteLine("birthdate1 : " + property.GetValue(validationContext.ObjectInstance).ToString());
                string date = property.GetValue(validationContext.ObjectInstance).ToString().Replace("-", "").Replace(".", "");

                //Console.WriteLine("birthdate2 : " + date);

                if (string.IsNullOrEmpty(date))
                {
                    // dummy 
                }
                else
                {
                    if (!Cmm.Utils.Utils.IsDate(date))
                    {
                        return new ValidationResult("유효한 날짜 형식이 아닙니다.");
                    }
                }

                property.SetValue(validationContext.ObjectInstance, date, null);
            }

            return ValidationResult.Success;
        }
    }

    // key-field forgery check  
    public class ZyHashKeyValid : ValidationAttribute
    {

        private string _property;

        public ZyHashKeyValid(string property)
        {
            _property = property;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = validationContext.ObjectInstance;
            var property = validationContext.ObjectType.GetProperty(_property);

            // 화면에서 넘어온 zyHashKey 
            var zyHashKeyProp = validationContext.ObjectType.GetProperty("ZyHashKey");
            string zyHashKey = Cmm.Utils.Utils.IsNull((zyHashKeyProp.GetValue(validationContext.ObjectInstance) as string), "");

            // 지정된 키값으로 생성한 hashkey
            string key = Cmm.Utils.Utils.IsNull((property.GetValue(validationContext.ObjectInstance) as string), "");
            string hashkey = Cmm.Utils.Utils.GetHashKey(key);

            Console.WriteLine("ZyHashKeyValid _property : " + (property.GetValue(validationContext.ObjectInstance) as string) + " / " + zyHashKey + " / " + hashkey);

            if (zyHashKey.Equals(hashkey))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("키값이 위변조되었습니다. [ZyHashKeyValid]");
            }
        }
    }

}



