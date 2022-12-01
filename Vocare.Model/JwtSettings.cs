using System;

namespace Vocare.Model
{
    public class JwtSettings
    {
        public string Secret { get; set; }
        public TimeSpan ExpiryTimeFrame { get; set; }
    }
}
