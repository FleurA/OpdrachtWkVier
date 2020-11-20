using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
/* 
 De opdracht:
Bij deze opdracht mag je creativiteit gebruiken om iets uit de werkelijkheid te modelleren. Maak eerst een klasse-diagram en vervolgens de bijbehorende C# code (met data-annotaties en de DbContext). Zorg dat Add-Migration en Update-Database werken.
Eisen:
Maak tenminste één een-op-een relatie aan. profiel -> artiest
Maak tenminste één een-op-veel relatie aan. 
Maak tenminste één veel-op-veel relatie aan.
Gebruik tenminste één keer overerving. Zie de docs.
Maak voor tenminste één configuratie gebruik van de Fluent API, i.p.v. de data-annotaties.
Maak tenminste één keer zinvol gebruik van een composite sleutel. Maak ook tenminste één foreign key naar deze sleutel. Zie de docs.
Maak tenminste een keer gebruik van [Table]
Maak tenminste een keer gebruik van [Column]
Maak tenminste een keer zinvol gebruik van [StringLength]
Maak tenminste een keer zinvol gebruik van [NotMapped]
 Resultaat:
Lever het UML diagram en de C# code in een .docx in.
 */

namespace OpdrachtWkVier
{
    // een artiest heeft een naam en kan gigs aanbieden 
  
    public class Artiest : User {

        private Artiest() { }
        public Artiest(User user)
        {
            name = user.name;
            this.Id = user.Id;
            this.profiel = new Profiel(this);
        }
        public Artiest(string naam)
        {
            base.name = naam;
            base.Id = new Guid();
            this.profiel = new Profiel(this);
        }
        public virtual List<Gig> Gigs { get; set; }

        public int werkgebiedInKm;

        public Locatie adres;

        public Profiel profiel;
}
// een user kan boekingen doen en boekingen en artiesten beoordelen 
public class User{
        public virtual Guid Id { get; set; }
        public virtual string name { get; set; }
        

}
    public class Boeker : User {

        private Boeker()
        {
            
        }
        public Boeker(User user, string iban)
        {
            // from base class 
            Id = user.Id;
            name = user.name;
           // from derived
            Iban = iban; 
        }
        public Boeker(string naam, string iban) {
            Id = new Guid();
            name = naam;
            Iban = iban;
        }
        string Iban { get; set; }
    }
    public class Profiel {
        private Profiel() { }
        public Profiel(Artiest artist)
        {
            this.Artiest = artist;

        }
        public Guid Id { get; set; }
        public   Artiest Artiest { get; set; }
        public string CustomCss { get; set; }
       [NotMapped]
        public string WelkomsVideoLink { get; set; }
    }
    
// een gig is een optreden met een omschrijving, een minimumduur en een prijs per kwartier. 
public class Gig{
        public Guid Id { get; set; }
        public double prijsPerKwartier { get; set; }

        public virtual List<Artiest> artiesten { get; set; }
        public string Naam { get; set; }

        public string Omschrijving { get; set; }
        [Column("duurInMin")]
        public  int duurInMinuten { get; set; }
   
}

public class Locatie{

        [Required] [StringLength(6)] public string postcode;
      
        public string huisnummer;

}
// een  boeking bestaat uit 1 boeker, 1 of meer artiesten en 1 of meer gigs
public class Boeking{

        private Boeking() { }
        public Boeking( Boeker boeker, List<Artiest> artiesten, List<Gig> gigs)
        {
            this.Boeker = boeker;
            this.GeboekteArtiesten = artiesten;
            this.Gigs = gigs;
        }
public Guid Id { get; set; }
public virtual Locatie locatie { get; set; }
[Required]
public Boeker Boeker { get; set; }
  [Required]
 
 public List<Artiest> GeboekteArtiesten { get; set; }
    [Required]
        public List<Gig> Gigs { get; set; }
}

public class MyContext : DbContext
{
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Locatie>()
                .HasKey(c => new { c.postcode, c.huisnummer });
            
                
        }


        protected override void OnConfiguring(DbContextOptionsBuilder b) =>
    b.UseSqlServer("Data Source=DESKTOP-RT99G2E\\SQLEXPRESS; Database=artiestdb;Trusted_Connection=True;");
        //b.UseSqlServer(@"Data Source=DESKTOP-RT99G2E\SQLEXPRESS;Initial Catalog=dbtest;Integrated Security=True");

    public DbSet<Artiest> Artiesten { get; set; }
    public DbSet<User> Users { get; set; }
        public DbSet<Boeker> Boekers { get; set; }
        public DbSet<Gig> Gigs { get; set; }
        public DbSet<Boeking> Boekingen { get; set; }

        public DbSet<Locatie> Locaties { get; set; }
    public DbSet<Profiel> Profielen { get; set; }

    }


    public static class DBInitializer
    {
        public static void Initialize(MyContext context)
        {
            if (context.Artiesten.Any()) return;

            Locatie DH = new Locatie() { postcode = "2516EG", huisnummer = "10A" };
            Locatie AM = new Locatie() { postcode = "1018BJ", huisnummer = "109" };
            context.Locaties.Add(DH); context.Locaties.Add(AM);

            Gig gig1 = new Gig() { prijsPerKwartier = 40.45, duurInMinuten = 60, Naam = "trompet spelen", Omschrijving = "Trompetist afgestudeerd aan het Conservatorium Den Haag" };
            Gig gig2 = new Gig() { prijsPerKwartier = 10, duurInMinuten = 60, Naam = "Bingo Host", Omschrijving = "Een gezellig potje bingo met host" };
            Gig gig3 = new Gig() { prijsPerKwartier = 19, duurInMinuten = 30, Naam = "Zang optreden", Omschrijving = "Een live zangconcert van dertig minuten. " };
            Gig gig4 = new Gig() { prijsPerKwartier = 39, duurInMinuten = 30, Naam = "Duo zang optreden", Omschrijving = "Een live zangconcert van dertig minuten door twee zangers. " };
            context.Gigs.Add(gig1); context.Gigs.Add(gig2); context.Gigs.Add(gig3); context.Gigs.Add(gig4);

            List<Gig> gigs1 = new List<Gig>();
            gigs1.Add(gig1); gigs1.Add(gig2);


            List<Gig> gigs2 = new List<Gig>();
            gigs2.Add(gig1); gigs2.Add(gig3); gigs2.Add(gig4);

            List<Gig> gigs3 = new List<Gig>();
            gigs3.Add(gig2); gigs3.Add(gig3); gigs3.Add(gig4);

            User art = new User() { name = "Houdinie" };
            User artt = new User() { name = "Frank Sinatra" };
           

            Artiest ar1 = new Artiest("Artiest1") { Gigs = gigs3, adres = DH, werkgebiedInKm = 200 };
            Artiest ar2 = new Artiest(art) { Gigs = gigs2, adres = DH, werkgebiedInKm = 20 };
            Artiest ar3 = new Artiest(artt) { Gigs = gigs3, adres = AM, werkgebiedInKm = 10 };
            Artiest ar4 = new Artiest("Bert&Ernie") { Gigs = gigs1, adres = AM };
            Artiest ar5 = new Artiest("Mr Celophane") { Gigs = gigs1, adres = AM, werkgebiedInKm = 200 };
            context.Artiesten.Add(ar1); context.Artiesten.Add(ar2); context.Artiesten.Add(ar3); context.Artiesten.Add(ar4); context.Artiesten.Add(ar5);

            List<Artiest> arts1 = new List<Artiest>() { };
            arts1.Add(ar2); arts1.Add(ar4); arts1.Add(ar5);
            List<Artiest> arts2 = new List<Artiest>() { };
            arts2.Add(ar1); arts2.Add(ar2); arts2.Add(ar3); arts2.Add(ar4); arts2.Add(ar5);
            List<Artiest> arts3 = new List<Artiest>() { };
            arts3.Add(ar1); arts3.Add(ar2); arts3.Add(ar3);
            List<Artiest> arts4 = new List<Artiest>() { };
            arts4.Add(ar1); arts4.Add(ar2); arts4.Add(ar3);
            List<Artiest> eenar = new List<Artiest>() { };
            eenar.Add(ar5);

            gig1.artiesten = arts1;
            gig2.artiesten = arts2;
            gig3.artiesten = arts3;
            gig4.artiesten = arts4;
            context.Gigs.Add(gig1);
            context.Gigs.Add(gig3);
            context.Gigs.Add(gig2);
            context.Gigs.Add(gig4);

            User user1 = new User() { name = "Peter" };
            User user2 = new User() { name = "E&G Evenementen" };
            User user3 = new User() { name = "Bob de Wit" };
            

            Boeker boeker1 = new Boeker(user1, "NL50BANK050435034453");
            Boeker boeker2 = new Boeker(user2, "NL44BANK583409453980");
            context.Boekers.Add(boeker1);
            context.Boekers.Add(boeker2);
            context.Users.Add(user3);

            Boeking boeking1 = new Boeking(boeker1, arts4, gigs2);
            Boeking boeking2 = new Boeking(boeker2, eenar, gigs2);
            context.Boekingen.Add(boeking1);
            context.Boekingen.Add(boeking2);

            context.SaveChanges();
        }
      
    }




    class Program
    {
        static void Main(String[] args)
        {
            Console.WriteLine("Hello World!");
            MyContext context = new MyContext();
            DBInitializer.Initialize(context);
           
            Console.WriteLine("Klaar met seeden. ");
           
           




        }

        



    }
    }

