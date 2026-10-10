// ===================== INTERFACES =====================
public interface ICurable
{
    void Curar();
}

public interface IDefendible
{
    void Defender();
}

// ===================== CLASE BASE ABSTRACTA =====================
public abstract class Personaje
{
    private const int VIDA_MAXIMA = 100;

    // Campos privados (encapsulamiento)
    private string _nombre;
    private int _vida;
    private int _nivel;

    // Propiedades
    public string Nombre
    {
        get { return _nombre; }
        private set { _nombre = value; }
    }

    // private set: nadie fuera de la clase puede modificar la vida libremente
    public int Vida
    {
        get { return _vida; }
        private set { _vida = value; }
    }

    public int Nivel
    {
        get { return _nivel; }
        private set { _nivel = value; }
    }

    // Constructor con validaciones
    public Personaje(string nombre, int vida, int nivel)
    {
        if (vida < 1 || vida > VIDA_MAXIMA)
            throw new ArgumentException("La vida inicial debe estar entre 1 y 100.");
        if (nivel < 1)
            throw new ArgumentException("El nivel inicial debe ser como mínimo 1.");

        Nombre = nombre;
        Vida = vida;
        Nivel = nivel;
    }

    // Método común para todos
    public void MostrarEstado()
    {
        Console.WriteLine($"Nombre: {Nombre}");
        Console.WriteLine($"Vida: {Vida}");
        Console.WriteLine($"Nivel: {Nivel}");
    }

    // Método abstracto: cada clase hija define cómo ataca
    public abstract void Atacar();

    public void RecibirDanio(int cantidad)
    {
        if (Vida == 0)
        {
            Console.WriteLine($"{Nombre} ya está derrotado.");
            return;
        }

        Vida -= cantidad;
        if (Vida < 0) Vida = 0;

        Console.WriteLine($"{Nombre} recibe {cantidad} puntos de daño.");
        Console.WriteLine($"Vida restante: {Vida}");

        if (Vida == 0)
            Console.WriteLine($"{Nombre} ha sido derrotado.");
    }

    // protected: solo las clases hijas pueden usarlo (para curarse sin pasar de 100)
    protected void RecuperarVida(int cantidad)
    {
        Vida += cantidad;
        if (Vida > VIDA_MAXIMA) Vida = VIDA_MAXIMA;
    }
}

// ===================== GUERRERO =====================
public class Guerrero : Personaje, IDefendible
{
    public int Fuerza { get; private set; }

    public Guerrero(string nombre, int vida, int nivel, int fuerza)
        : base(nombre, vida, nivel)
    {
        Fuerza = fuerza;
    }

    public override void Atacar()
    {
        Console.WriteLine($"{Nombre} ataca con su espada causando {Fuerza} puntos de daño.");
    }

    public void Defender()
    {
        Console.WriteLine($"{Nombre} levanta su escudo y se prepara para defenderse.");
    }
}

// ===================== MAGO =====================
public class Mago : Personaje, ICurable
{
    public int Mana { get; private set; }

    public Mago(string nombre, int vida, int nivel, int mana)
        : base(nombre, vida, nivel)
    {
        Mana = mana;
    }

    public override void Atacar()
    {
        Console.WriteLine($"{Nombre} lanza un hechizo causando 40 puntos de daño.");
    }

    public void Curar()
    {
        int vidaAntes = Vida;
        RecuperarVida(20);
        Console.WriteLine($"{Nombre} utiliza magia de curación.");
        Console.WriteLine($"Vida recuperada: {Vida - vidaAntes}");
        Console.WriteLine($"Vida actual: {Vida}");
    }
}

// ===================== ARQUERO =====================
public class Arquero : Personaje
{
    public int Flechas { get; private set; }

    public Arquero(string nombre, int vida, int nivel, int flechas)
        : base(nombre, vida, nivel)
    {
        Flechas = flechas;
    }

    public override void Atacar()
    {
        if (Flechas == 0)
        {
            Console.WriteLine($"{Nombre} no tiene flechas y no puede disparar.");
            return;
        }

        Flechas--;
        Console.WriteLine($"{Nombre} dispara una flecha causando 25 puntos de daño.");
        Console.WriteLine($"Flechas restantes: {Flechas}");
    }
}

// ===================== PROGRAMA PRINCIPAL =====================
public class Program
{
    // Espera a que el usuario presione ENTER para continuar
    private static void Pausar()
    {
        Console.WriteLine("(Presiona ENTER para continuar...)");
        Console.ReadLine();
        Console.WriteLine();
    }

    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Guerrero aquiles = new Guerrero("Aquiles", 100, 5, 30);
        Mago gandalf = new Mago("Gandalf", 80, 6, 100);
        Arquero katniss = new Arquero("Katniss", 90, 4, 10);

        // Polimorfismo: una colección de la clase base
        Personaje[] personajes = { aquiles, gandalf, katniss };

        Console.WriteLine("===== PERSONAJES =====\n");

        foreach (Personaje personaje in personajes)
        {
            personaje.MostrarEstado();
            Console.WriteLine();
            personaje.Atacar(); // comportamiento distinto según el objeto real
            Console.WriteLine();
            Pausar();
        }

        Console.WriteLine("===== HABILIDADES =====\n");

        aquiles.Defender();
        Pausar();

        gandalf.RecibirDanio(30);
        Pausar();

        gandalf.Curar();
        Pausar();

        katniss.Atacar();
        Pausar();

        katniss.Atacar();
        Pausar();

        katniss.Atacar();
        Pausar();

        aquiles.RecibirDanio(30);

        Console.WriteLine("\nFin del programa. Presiona ENTER para salir...");
        Console.ReadLine();
    }
}