namespace TrustYourBrand.Models
{
    public class TenantDto
    {
        public int TenantId { get; set; }
        public string Name { get; set; } // Nome da Empresa
        public string ContribuinteNif { get; set; } // NIF
        public bool IsActive { get; set; }
        public string Email { get; set; } // E-mail
        public string Phone { get; set; } // Telefone
        public string Country { get; set; } // Country
        public string City { get; set; } // City
        public string Street { get; set; } // Rua
        public string Sector { get; set; } // Sector
        public int NumberOfEmployees { get; set; } // N. Funcionários
        public int CompanyYear { get; set; } // Ano da Empresa
        public string LinkedIn { get; set; } // Linkedin
        public string Website { get; set; } // Web site
        public DateTime SubscriptionStartDate { get; set; } // Data Início Subscrição
        public DateTime SubscriptionEndDate { get; set; } // Data Fim Subscrição
        public byte[] LogoData { get; set; } // Logo Tipo (caminho do arquivo)

    }
}
