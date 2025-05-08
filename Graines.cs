public class Graines
{
    public int Nombre { get; private set; }

    public Graines(int initial = 50)
    {
        Nombre = initial;
    }

    public bool PeutDepenser(int montant)
    {
        return montant <= Nombre;
    }

    public void Depenser(int montant)
    {
        Nombre -= montant;
    }

    public void Ajouter(int montant)
    {
        Nombre += montant;
    }
    public override string ToString()
    {
        return Nombre.ToString();
    }
}