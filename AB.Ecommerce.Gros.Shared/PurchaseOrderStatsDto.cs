using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AB.Ecommerce.Gros
{
    public class PurchaseOrderStatsDto
    {
        public int TotalOrders { get; set; }
        public double TotalAmount { get; set; }
        public double TotalPaied { get; set; }
        public double TotalRest { get; set; }
        public double AverageOrderValue { get; set; }

        // Répartition par statut de commande (ex: "Pending": 5, "Received": 10)
        public Dictionary<string, int> OrdersByStatus { get; set; } = new();

        // Répartition par statut de paiement (ex: "Unpaid": 3, "Paid": 12)
        public Dictionary<string, int> OrdersByPaymentStatus { get; set; } = new();
    }
}
