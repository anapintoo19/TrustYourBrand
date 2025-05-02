namespace TrustYourBrand.Services
{
    public class CultureChangeService
    {
        public event Action OnCultureChanged;

        public void NotifyCultureChanged()
        {
            OnCultureChanged?.Invoke();
        }
    }
}