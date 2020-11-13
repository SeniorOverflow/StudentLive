using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;

namespace StudentLivingOn1000Rub
{
     delegate void UseItemOnStudent(int idItem, int IdStudent);
    enum FemaleNames
    {
        Арина,
        Алиса,
        Анастасия,
        Анна,
        Аделина,
        Виктория,
        Василиса,
        Вероника,
        Евгения,
        Елизавета,
        Дарья,
        Ксения,
        Кира,
        Татьяна,
        Ева,
        София,
        Мария,
        Милана
    }
    enum MaleNames
    { 
        Андрей,
        Артем, 
        Александр, 
        Давид, 
        Дмитрий,
        Данила, 
        Лев,
        Михаил,
        Марк,
        Матвей, 
        Роман,
        Сергей,
        Степан, 
        Семен
    };
    

    
    class Item
    {
        static int count = 0;
        int Id;
        string Name;
        int Price;
        int HealAdd;
        int PowerAdd;
        int RemoveHungry;


        public Item(string name , int price , int healAdd , int powerAdd, int removeHungry)
        {
            Id = count;
            Name = name;
            Price = price;
            HealAdd = healAdd;
            PowerAdd = powerAdd;
            RemoveHungry = removeHungry;
            count++;
        }
        public int GetId => Id;
        public string GetName => Name;
        public int GetPrice => Price;
        public int GetHealAdd => HealAdd;
        public int GetPowerAdd => PowerAdd;
        public int GetRemoveHungry => RemoveHungry;

    }

    class Student
    {
        static int total = 0;
        int Id;
        string Name;
        int Money;
        int Reputation;
        int Heal;
        int Power;
        int Hungry;
        int Agility;
        int Intelligence;
        bool Gender;
        bool IsPlayer;
        bool IsDead;
        static int maxMoney = 12000;
        static int maxReputation = 5000;
        static int maxHeal = 1000;
        static int maxPower = 1000;
        static int maxHungry = 2000;
        static int maxAgility = 200;
        static int maxIntelligence = 280;

        List<InventoryItem> Inventory;

        public bool CheckPlayer() => IsPlayer;
        public bool CheckDead() => IsDead;
        struct InventoryItem
        {
             int Id;
             int Count;
           public  InventoryItem(int id, int count)
            {
                this.Id = id;
                this.Count = count;
            }
            public int GetId =>Id;
            public int GetCount => Count;
            public void Add(int count) => Count += count;
            public bool Remove(int count = 1)
            {
                if (Count <= count)
                {
                    Count -= count;
                    return true;
                }
                return false;
            }
        }
        public Student(string name,
                bool gender,
                bool isPlayer = false,
                int money = 1000, 
                int reputation = 0, 
                int heal = 500, 
                int power = 1000, 
                int hungry = 0, 
                int intelligence = 70)
        {

            Inventory = new List<InventoryItem> ();
            this.IsPlayer = isPlayer;
            this.IsDead = false;
            this.Gender = gender;
            this.Intelligence = intelligence;
            this.Name = name;
            this.Heal = heal;
            this.Money = money;
            this.Reputation = reputation;
            this.Power = power;
            this.Hungry = hungry;
            Id = total;
            total++;
        }
        public bool UseItem(int idItem)
        {
            for (int i = 0; i < Inventory.Count; i++)
                if (Inventory[i].GetId == idItem)
                    if (Inventory[i].Remove())
                    {
                        return true;
                    }
            return  false;
        }
        public void AddItemStat(Item item)
        {
            
       
            Console.WriteLine(@$"You use {item.GetName}");
            Heal += item.GetHealAdd;
            Power += item.GetPowerAdd;
            Hungry -= item.GetRemoveHungry;
        }
        public void AddItem(int id , int count)
        {
            for(int i = 0; i < Inventory.Count; i++)
                if (Inventory[i].GetId == id)
                {
                    Inventory[i].Add(count);
                    return;
                }
            var item = new InventoryItem(id, count);
            Inventory.Add(item);
        }
        public bool Pay(int price )
        {
            if(price <= Money)
            {
                Money -= price;
                return true;
            }
            return false;
        }

        void EndGame(Action endGameFunction)
        {
            endGameFunction();
        }
        public void PrintStats()
        {
            Console.WriteLine(@$"
            name:   {Name}  
            money:  {Money}  
            heal:   {Heal} 
            power:  {Power}  
            hugry:  {Hungry}");
        }
        
       
       public void  CheckAndReplaceStats()
        {
            WasteEnergy();
           
            if (Reputation > maxReputation) Reputation = maxReputation;
            if (Heal > maxHeal) Heal = maxHeal;
            if (Power > maxPower) Power = maxPower;
            if (Hungry > maxHungry)
                Hungry = maxHungry;

            if(Heal <= 0 )
                EndGame(HungryEnd);
            if (Agility == 0)
                EndGame(AgilityEnd);
            if (Intelligence == 0)
                EndGame(IntelligenceEnd);
        }

        public void WasteEnergy()
        {
            Hungry += 50;
            if(Hungry > 1500)
            {
                if (Hungry > 1900)
                    Heal -= 100;
                else
                    Heal -= 10;
            }
            Power -= 20;
        }
        void GoodEnd()
        {
        }
        void HungryEnd()
        {
            var message = Gender ? "Умер от голода" : "Умерла от голода";
            IsDead = true;
            Console.WriteLine(@$"{Name} {message }");
        }
        void AgilityEnd()
        {
        }
        void IntelligenceEnd()
        {
        }
    }
    class Place
    {
        int id;
        int count = 0;
        string Name;
        List<Action> actions;
        List<Student> studentsInPlace;
        public Place(string name,List<Action> _actions)
        {
            Name = name;
            actions = new List<Action>();
            studentsInPlace = new List<Student>();
            foreach(var action in _actions)
                actions.Add(action);
            id = count;
            count++;
        }
        public int GetId() => id;
        public Action GetActionByID(int idAction) => actions[idAction];
        public string GetName() => Name;
        public void ShowAllActions()
        {
            for(int i = 0; i < actions.Count; i++)
                Console.WriteLine($@"{i }  -  {actions[i].Method.Name}");  
        }
        public Action GetActionById(int id) => actions[id];

    }
    public class World
    {
      //  int CountStudents = 25;
        int idCurrentPlace = 0;
        int countTics = 0;
       
        List<Student> students;
        List<Place> places;
        Item[] Items =
        {
            new Item("Дошик",20 , 0 , 0 , 200 ),
            new Item("Кофе",110 , 100 , 100 , 0),
            new Item("Сендвич из Автомата",50 , 50, 0, 500 ),
            new Item("Яблоко",10 , 0 , 0 , 50 ),
         };
        public World()
        {
            students = new List<Student>();
            places = new List<Place>();
            countTics = 0;
            //XXXX -BEGIN- please make me cool
            var worldActions = new List<Action>();
            worldActions.Add(ChangePlace);
            worldActions.Add(WorldShop); //KeepTurn
            worldActions.Add(KeepTurn);
            var world = new Place("World", worldActions);

            var gukActions = new List<Action>();
            gukActions.Add(ChangePlace);
            gukActions.Add(GukShop);
            gukActions.Add(KeepTurn);
            var guk = new Place("Guk", gukActions);

            var UgiActions = new List<Action>();
            UgiActions.Add(ChangePlace);
            UgiActions.Add(UgiShop);
            UgiActions.Add(KeepTurn);
            var ugi = new Place("Ugi", UgiActions);

            var radikActions = new List<Action>();
            radikActions.Add(ChangePlace);
            radikActions.Add(RadikShop);
            radikActions.Add(KeepTurn);
            var radik = new Place("Radik", radikActions);

            var matMexActions = new List<Action>();
            matMexActions.Add(ChangePlace);
            matMexActions.Add(MatMexShop);
            matMexActions.Add(KeepTurn);
            var matMex = new Place("MatMex", matMexActions);

            places.Add(world);
            places.Add(guk);
            places.Add(ugi);
            places.Add(radik);
            places.Add(matMex);
            //XXXX -END- please make me cool
            idCurrentPlace = world.GetId();
         //   CreateStudents(CountStudents);
        }
        void AddStudent(Student student) => students.Add(student);
        void AddPlace(Place place) => places.Add(place);


        public void ShowPlaces()
        {
            for (int i = 0; i < places.Count; i++)
                Console.WriteLine($@"{i }  -  {places[i].GetName()}");
        }
        void ChangePlace()
        {
            while (true)
            {
                ShowPlaces();
                Console.Write("Write needed place number :");
                var answer = Console.ReadLine();
                Console.WriteLine();
                if (IsStringNumber(answer))
                {
                    idCurrentPlace = Int32.Parse(answer);
                    break;
                }
                else
                {
                    Console.WriteLine("Not a num place, im so sorry");
                }
            }
        }
        void KeepTurn() { }
       
        void GukShop() =>Shop(2);
        
        void UgiShop() => Shop(1.5);
        
        void RadikShop() =>Shop(1.1);
        
        void MatMexShop() => Shop(1.2);
        
        void WorldShop() => Shop(5); 

       void  ShowShopMenu(double multiple)
        {
           for(int i = 0; i < Items.Length;i ++)
            {
                Console.WriteLine($@"      {i} - {Items[i].GetName}  price: {Items[i].GetPrice  * multiple} ");
            }
        }
        void Shop(double multiple = 1)
        {
            while (true)
            {
                ShowShopMenu(multiple);
                var answer = Console.ReadLine();
                if(IsStringNumber(answer))
                {
                    foreach (var student in students)
                        if (student.CheckPlayer())
                        {
                            var item = Items[Int32.Parse(answer)];
                            if (student.Pay((int)(multiple * item.GetPrice)))
                            {
                                student.AddItem(item.GetId, 1);
                                if (student.UseItem(item.GetId))
                                {
                                    Console.Clear();
                                    var time = countTics % 24;
                                    Console.WriteLine($@"Day : {countTics / 24}  Time: {time}:00");
                                    student.PrintStats();
                                    student.AddItemStat(item);
                                }
                            }
                            else
                            {
                                Console.Clear();
                                var time = countTics % 24;
                                Console.WriteLine($@"Day : {countTics / 24}  Time: {time}:00");
                                student.PrintStats();
                                Console.WriteLine("You don't have money");

                                break;
                            }
                        }
                   
                }
                else
                {
                    if (answer == "q" || answer == "Q")
                        break;
                }
            }

        }


        bool Tic()
        {
            Console.Clear();
            var time = countTics % 24;
            Console.WriteLine($@"Day : {countTics / 24}  Time: {time}:00");
            foreach (var student in students)
                if (student.CheckPlayer())
                {
                    student.PrintStats();
                    break;
                }

            Console.WriteLine("New Tic");
            var place = places[idCurrentPlace];
            ref Place refPlace = ref place;
            Console.WriteLine(place.GetName());
            var action = CallPlasceActionsMenu(ref refPlace);
            action();
            foreach (var student in students)
            {
                student.CheckAndReplaceStats();
                if (student.CheckPlayer() && student.CheckDead())
                    return false;
            }
            countTics++;
            return true;
        }
        

        bool IsStringNumber(string number)
        {
            foreach (char letter in number)
                if (char.IsNumber(letter) == false)
                    return false;
            return true;
        }
        Action CallPlasceActionsMenu(ref Place place)
        {
            while (true)
            {
                place.ShowAllActions();
                var answer = Console.ReadLine();

                if (answer == "") return KeepTurn;

                if (IsStringNumber(answer) && answer != "")
                {
                    var idAction = Int32.Parse(answer);
                    var action = place.GetActionByID(idAction);
                    return action;
                }
                
            }
        }
       
        void CreateStudents(int count)
        {
            Random random = new Random();
            
            for(int i = 0; i < count; i++)
            {
                var gender = random.Next(100) <= 50 ? true : false;
                var name = "";
                if (gender)
                     name = "" +(MaleNames)random.Next(Enum.GetValues(typeof(MaleNames)).Length);
                else
                    name = "" + (FemaleNames)random.Next(Enum.GetValues(typeof(FemaleNames)).Length);

                var student = new Student(name, gender);
                AddStudent(student);
            }
        }
        public void StartGame()
        {
            int countTic = 0;
            while (Tic()) {
                //если проходит 24 тика то мы вызываем 
                 Console.WriteLine(@$"Day {countTic % 24}");
                if (countTic % 24 == 0)
                {
                    Console.WriteLine(@$"Day {countTic % 24}");
                }
                countTic++;

            }

        }
        public void CreatePlayer()
        {
            Console.Write("Write your name : ");
            string name = Console.ReadLine();
            Console.WriteLine();
            bool gender;
            while(true)
            {
                Console.Write("Write your gender - 1 is male, 0 is female : ");
                var genderAnswer = Console.ReadLine();
                Console.WriteLine();
                if(genderAnswer == "1" || genderAnswer == "0")
                {
                    if (genderAnswer == "1")
                        gender = true;
                    else
                        gender = false;
                    break;
                }
            }
            var student = new Student(name, gender, true);
        
            AddStudent(student);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var world = new World();
           
            world.CreatePlayer();
            world.StartGame();
            Console.ReadKey();
        }
    }
}
