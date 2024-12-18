using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

public class Program {

    public static string path = @"..\..\..\..\Gestionnaire\MDP.txt";

    public static void PresentationGestionnaire() {

        Console.Write("\n" + @"Bienvenue dans le gestionnaire de mot de passe. Que souhaitez vous faire ? :
        1: Afficher la liste des mots de passes
        2: Creer un mot de passe
        3: Supprimer un ou plusieurs mot de passes
        4: Copier un mot de passe au presse papier
        5: Quitter le programme" + "\n\n");
    }

    public static void AffichageMdpNumerote(string[] lignes) {

        int tailleNom = 0;

        for (int i = 1; i < lignes.Length; i++) {
            if (!string.IsNullOrEmpty(lignes[i]) && char.IsLetter(lignes[i][0])) {
                tailleNom = lignes[i].IndexOf(":");
                Console.WriteLine($"{(i / 3).ToString().PadRight(3)} : {lignes[i].Substring(0, tailleNom).Trim()}");
            }
        }
        Console.WriteLine("\n");
    }

    public static bool VerificationNom(string name) {

        string[] lignes = File.ReadAllLines(path);

        foreach (string ligne in lignes) {

            if (ligne.StartsWith($"{name} :"))
                return false;
        }

        return true;
    }

    public static void CreationMotDePasse() {

        string? name;
        string motDePasse = "";
        char[] alphabet = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'];
        char[] alphabetMaj = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        char[] chiffres = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
        char[] symboles = ['$', '#', '*', '!', '%', '&'];
        char[] caracteresSpeciaux = ['!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '=', '+','[', ']', '{', '}', ';', ':', '\'', '"',
            ',', '.', '<', '>', '/', '?', '\\', '|', '`', '~'];

        Random rand = new Random();
        bool sortieBoucle = false;

        Console.Write("Nom à utiliser pour le mot de passe (interdiction aux caractères spéciaux) : ");

        do {
            name = Console.ReadLine();
        } while (string.IsNullOrEmpty(name));

        while (name!.Any(c => caracteresSpeciaux.Contains(c)) || !VerificationNom(name!)) {

            while (!VerificationNom(name!)) {
                string[] reponsesValidation = ["y", "yes", "o", "oui", "1"];
                string[] reponsesRefus = ["n", "non", "no", "2"];
                Console.Write(@"Ce nom existe deja, veux tu le remplacer ? :
            1 : Oui
            2 : Non" + "\n\n");

                string? reponse = Console.ReadLine()?.Trim().ToLower();

                if (reponse != null && reponsesValidation.Contains(reponse)) {
                    sortieBoucle = true;
                    break;
                }

                else if (reponse != null && reponsesRefus.Contains(reponse))
                    break;

                else
                    Console.WriteLine("Reponse invalide");
            }

            if (sortieBoucle)
                break;

            Console.Write("Veuillez mettre un nom valide : ");
            name = Console.ReadLine();
        }

        do {
            motDePasse = "";

            for (int i = 0; i < rand.Next(15, 26); i++) {

                switch (rand.Next(1, 16)) {

                    case <= 4:
                        motDePasse += alphabet[rand.Next(alphabet.Length)];
                        break;
                    case <= 8:
                        motDePasse += alphabetMaj[rand.Next(alphabetMaj.Length)];
                        break;
                    case <= 12:
                        motDePasse += chiffres[rand.Next(chiffres.Length)];
                        break;
                    case <= 15:
                        motDePasse += symboles[rand.Next(symboles.Length)];
                        break;
                    default:
                        break;
                }
            }
        } while (!(motDePasse.Any(c => alphabet.Contains(c)) && motDePasse.Any(c => alphabetMaj.Contains(c))
                && motDePasse.Any(c => chiffres.Contains(c)) && motDePasse.Any(c => symboles.Contains(c))));

        Console.WriteLine($"Mot de passe généré pour {name} : {motDePasse}");

        if (sortieBoucle) {

            string[] lignes = File.ReadAllLines(path);

            for (int i = 0; i < lignes.Length; i++) {

                if (lignes[i].StartsWith($"{name} :"))
                    lignes[i] = $"{name} : {motDePasse}";
            }
            File.WriteAllLines(path, lignes);
        }
        else
            File.AppendAllLines(path, new[] { $"{name} : {motDePasse} \n\n" });
    }

    public static void SupprimerMotDePasse() {

        string[] lignes = File.ReadAllLines(path);
        List<string> lignesListe = new List<string>(lignes);

        if (lignes.Length < 4) {

            Console.WriteLine("Vous n'avez aucun mot de passe à supprimer");
            return;
        }

        Console.Write("\n" + @"Voulez supprimer un mot de passe? Ou tous les mot de passes ? :
        1: Un seul
        2: Tous" + "\n\n");

        string? reponse;

        do {
            reponse = Console.ReadLine();
            Console.Write("\n");
            if (string.IsNullOrEmpty(reponse))
                Console.WriteLine("Veuillez donner une reponse");
            else if (reponse != "1" && reponse != "2")
                Console.WriteLine("Veuillez mettre une des reponses propose");
        } while (reponse != "1" && reponse != "2");

        Console.Clear();

        if (reponse == "1") {

            Console.WriteLine("Voici une liste de tous les mots de passes : \n");

            AffichageMdpNumerote(lignes);

            Console.WriteLine("\nVeuillez maintenant choisir le fichier à supprimer");
            int reponseInt = 0;

            do {
                reponse = Console.ReadLine();
                Console.Write("\n");
                if (string.IsNullOrEmpty(reponse))
                    Console.WriteLine("Veuillez donner une reponse");
                else if (!int.TryParse(reponse, out reponseInt) || reponseInt < 1 || reponseInt > lignes.Length / 3)
                    Console.WriteLine("Veuillez mettre une des reponses propose");
            } while (!int.TryParse(reponse, out reponseInt) || reponseInt < 1 || reponseInt > lignes.Length / 3);

            lignesListe.RemoveRange((reponseInt * 3), 3);

            File.WriteAllLines(path, lignesListe);

            Console.WriteLine("Mot de passe supprimé.");
        }

        else if (reponse == "2") {

            lignesListe.RemoveRange(3, lignes.Length - 3);

            File.WriteAllLines(path, lignesListe);

            Console.WriteLine("Mots de passe supprimés.");
        }

    }

    public static void CopierMdp() {

        string? reponse;
        int reponseInt = 0;
        string[] lignes = File.ReadAllLines(path);

        if (lignes.Length < 4) {

            Console.WriteLine("Vous n'avez aucun mot de passe à copier");
            return;
        }

        Console.WriteLine("Voici la liste des mots de passe, lequel voulez vous supprimer ?\n");
        AffichageMdpNumerote(lignes);

        do {
            reponse = Console.ReadLine();
            Console.Write("\n");
            if (string.IsNullOrEmpty(reponse))
                Console.WriteLine("Veuillez donner une reponse");
            else if (!int.TryParse(reponse, out reponseInt) || reponseInt < 1 || reponseInt > lignes.Length / 3)
                Console.WriteLine("Veuillez mettre une des reponses propose");
        } while (!int.TryParse(reponse, out reponseInt) || reponseInt < 1 || reponseInt > lignes.Length / 3);

        int tailleNom = lignes[reponseInt*3].IndexOf(":");

        Clipboard.SetText(lignes[reponseInt*3].Substring(tailleNom + 1).Trim());
    }

    [STAThread]
    static void Main() {

        int resultat = 0;
        string? input;

        if (!Directory.Exists("..\\..\\..\\..\\Gestionnaire\\Gestionnaire"))
            Directory.CreateDirectory("..\\..\\..\\..\\ProjetsPerso\\Gestionnaire\\Gestionnaire");

        if (!File.Exists(path)) {
            using (StreamWriter sw = File.CreateText(path))
                sw.WriteLine("Mots de passe existants :\n\n");
        }

        do {
            PresentationGestionnaire();

            input = Console.ReadLine();

            while (!Int32.TryParse(input, out resultat) || (resultat != 1 && resultat != 2 && resultat != 3 && resultat != 4 && resultat != 5)) {
                Console.WriteLine("Entrée invalide. Veuillez entrer un nombre entre 1 et 4 :");
                input = Console.ReadLine();
            }

            Console.Clear();

            switch (resultat) {
                case 1:
                    using (StreamReader sr = File.OpenText(path)) {
                        string? s;
                        while ((s = sr.ReadLine()) != null) {
                            Console.WriteLine(s);
                        }
                    }
                    break;
                case 2:
                    CreationMotDePasse();
                    break;
                case 3:
                    SupprimerMotDePasse();
                    break;
                case 4:
                    CopierMdp();
                    break;
                case 5:
                    Console.WriteLine("Fermeture du programme...");
                    return;
                default:
                    Console.WriteLine("Option non reconnue.");
                    break;
            }
        } while (true);
    }
}