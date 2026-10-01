using System;
namespace AB.Ecommerce.Gros.Business
{
	public static class BusinessErrors
	{

		public const string ProductNotFound = "Product:001";
        public const string StockNotSuffiscient = "Product:002";
        public const string MinimumQuantiyOrderConstraint = "Product:003";
        public const string MaximumQuantiyOrderConstraint = "Product:004";
        public const string ProductCodeAlreadyExists = "Product:005";
        public const string ProductLabelAlreadyExists = "Product:006";
        public const string BottleConfigurationNotFound = "Product:007";
        public const string ProductVariantNotFound = "Product:008";
        public const string BottlePriceNotFound = "Product:009";
        public const string ProductStockNotFound = "Product:010";
        public const string ManualSanctionQuantityRequired = "Product:011";
        public const string ManualSanctionPriceRequired = "Product:012";

        public const string RequiredField = "Arguments:001";
        public const string MinPasswordLength = "Arguments:002";
        public const string MaxPasswordLength = "Arguments:003";

        public const string ClientHasSavedOrders = "Client:001";

        public const string RoleAlreadyExists = "Role:001";
        public const string RoleDoesntExist = "Role:002";
        public const string UnableToCreateRole = "Role:003";
        public const string UnableToUpdateRole = "Role:004";

        public const string OrderNotCancelled = "Order:001";
        public const string OrderEmpty = "Order:002";



        public static Dictionary<string, string> L = new Dictionary<string, string>()
		{
			{ ProductNotFound, "Produit non existant"  },

            { StockNotSuffiscient, "Le stock n'est pas suffisant"  },

            { MinimumQuantiyOrderConstraint, "Il faut commander au moins {0} unités"  },

            { MaximumQuantiyOrderConstraint, "Il faut commander au maximum {0} unités"  },

            { ProductCodeAlreadyExists, "Un produit avec le code '{0}' existe déjà. Veuillez utiliser un autre code." },

            { ProductLabelAlreadyExists, "Un produit avec le libellé '{0}' existe déjà. Veuillez utiliser un autre libellé." },

            { BottleConfigurationNotFound, "Le flacon sélectionné est introuvable." },

            { ProductVariantNotFound, "La variante sélectionnée n'appartient pas à ce produit." },

            { BottlePriceNotFound, "Aucun prix n'est configuré pour ce flacon et cette variante." },

            { ProductStockNotFound, "Aucun stock n'est configuré pour cette variante dans le magasin '{0}'." },

            { ManualSanctionQuantityRequired, "La quantité de la sanction est obligatoire et doit être supérieure à zéro lorsqu'aucun flacon n'est sélectionné." },

            { ManualSanctionPriceRequired, "Le prix de la sanction est obligatoire et doit être positif lorsqu'aucun flacon n'est sélectionné." },

            { RequiredField, "Le champs '{0}' est obligatoire !"  },

            { MinPasswordLength, "Le mot de passe doit contenir au moins {0} caractéres"  },

            { MaxPasswordLength, "Le mot de passe doit contenir au maximum {0} caractéres"  },

             { ClientHasSavedOrders, "Le client a des commandes enregistrées"  },


             { RoleAlreadyExists, "Ce rôle existe déja !"  },
             { RoleDoesntExist, "Ce rôle n'existe pas !"  },
             { UnableToCreateRole, "Impossible de créer ce rôle"  },
             { UnableToUpdateRole, "Impossible de modifier ce rôle"  },

             { OrderNotCancelled, "la commande n'est pas annule"  },
             { OrderEmpty,"La commande est Vide"}

        };

    }
}

