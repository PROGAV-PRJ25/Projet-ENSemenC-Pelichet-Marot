class Program
{
    static void Main()
    {
        var plateau = GenerateurPlateau.GenererPlateau(10, 10);
        var gestionnaire = new GestionnairePotager();
        gestionnaire.AfficherPlateau(plateau);
    }
}
