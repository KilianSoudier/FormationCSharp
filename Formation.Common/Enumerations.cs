namespace Formation.Common
{
    public class Enumerations
    {
        public enum TypeEchantillon
        {
            Sang,
            Tissu,
            ADN,
            RNA,
            Plasma,
            Autre //Peut être utilisé pour les échantillons qui ne correspondent pas aux catégories prédéfinies
        }

        public enum StatutEchantillon
        {
            EnAnalyse,
            AnalyseComplete,
            EnStockage,
            Expedie,
            Detruit //Pour les échantillons qui ne sont plus disponibles
        }
    }
}
