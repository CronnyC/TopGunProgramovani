using System;
using System.Collections.Generic;

class Program
{
  //crash testy  
  public static void crashTest(){
    Console.WriteLine("\n\n\n\nCRASH TESTS\n\n");
    
    //init tricky stuff
    Character player3 = new Character("Tester");
    Weapon impossibleSword = new Weapon("Very Heavy Sword", 500, 10, 1);
    Weapon twoHandedWeapon1 = new Weapon("Two-Handed Sword 1", 10, 10, 2);
    Weapon twoHandedWeapon2 = new Weapon("Two-Handed Sword 2", 10, 10, 2);
    Armor heavyArmor = new Armor("Heavy Armor", 40, 10);
    Shield heavyShield = new Shield("Heavy Shield", 40, 10);

    //setting custom values so i can test it more accurately
    player3.Strength=5;
    player3.Agility=0;

    Console.WriteLine("\n\nEquip what is not in inventory\n");
    player3.EquipWeapon(twoHandedWeapon2, null); 
    player3.EquipShield(heavyShield, twoHandedWeapon2);
    player3.EquipArmor(heavyArmor);

    Console.WriteLine("\n\nPrint out empty inventory\n");
    player3.PrintInventory();

    Console.WriteLine("\n\nAdd to inventory very heavy sword\n");
    player3.Inventory.AddItem(impossibleSword, player3);

    Console.WriteLine("\n\nUnequip when nothing is equipped\n");
    player3.UnequipWeapon(); 
    player3.UnequipShield();
    player3.UnequipArmor();

    Console.WriteLine("\n\nAdd to the very max value, show with a single copper being over the limit\n");
    player3.Inventory.AddItem(twoHandedWeapon1, player3);
    player3.Inventory.AddItem(twoHandedWeapon2, player3);
    player3.Inventory.AddItem(heavyArmor, player3);
    player3.Inventory.AddItem(heavyShield, player3);
    player3.addMoney(0,0,1);
  
    Console.WriteLine("\n\nEquipping when over limit\n");
    player3.EquipWeapon(twoHandedWeapon2, null); 
    player3.EquipShield(heavyShield, twoHandedWeapon2);
    player3.EquipArmor(heavyArmor);

    player3.Strength=200;
    Console.WriteLine("\n\nEquipping two two-handed weapons or shield and two-handed weapon (after increasing max load)\n");
    player3.EquipWeapon(twoHandedWeapon1, null);
    player3.EquipWeapon(twoHandedWeapon2, null);
    player3.EquipShield(heavyShield, twoHandedWeapon2);
    player3.PrintInventory();

    Console.WriteLine("\n\nTest unequipping items\n");
    player3.UnequipWeapon();
    player3.UnequipShield();
    player3.UnequipArmor();
    
    Console.WriteLine("\n\nProve with inventory\n");
    player3.PrintInventory();

    Console.WriteLine("\n\nCrash test ended, thank you for attention :)\n");
  }

  public static void Main(string[] args){
    //init players - for demonstration purposes input already set
    //Console.Write("Pick first character name: ");
    //Character player1 = new Character(Console.ReadLine());

    //Console.Write("Pick second character name: ");
    //Character player2 = new Character(Console.ReadLine());

    Character player1 = new Character("Pepíno");
    Character player2 = new Character("Ten druhej klučina");

    //init items
    Weapon ironSword = new Weapon("Iron Sword", 3, 10, 1);
    Weapon bigAxe = new Weapon("Big Ol' Axe", 8, 18, 2);
    Weapon dagger = new Weapon("Sneaky Little Blade", 0.2, 5, 1);
    Shield woodenShield = new Shield("Wooden Shield", 5, 5);
    Armor leatherArmor = new Armor("Leather Armor", 15, 7);

    //add stuff to inventory
    player1.Inventory.AddItem(ironSword,player1);
    player1.Inventory.AddItem(woodenShield, player1);
    player1.Inventory.AddItem(leatherArmor, player1);
    player1.addMoney(3,2,8);

    //equip stuff
    player1.EquipWeapon(ironSword, player1.equippedShield);
    player1.EquipShield(woodenShield, player1.equippedWeapon);
    player1.EquipArmor(leatherArmor);

    //print out inventory
    player1.PrintInventory();
    player2.PrintInventory();

    //attack loop
    while(player1.MaxHP>0 && player2.MaxHP>0){
      player1.Attack(player2);
      if(player2.MaxHP>0)
        player2.Attack(player1);
    }
    if(player1.MaxHP<=0)
      Console.WriteLine($"{player1.Name} was agressivly slaughtered by {player2.Name} and is no longer among us.");
    if(player2.MaxHP<=0)
      Console.WriteLine($"{player2.Name} was agressivly slaughtered by {player1.Name} and is no longer among us.");
    
    crashTest();
  }
}

class Character{
  //character properties (max load= strength*20)
  //inventory is sort of detached, it is a cart being dragged behind him and when equipped, it is half the load (when character's max load is 200kg and "on cart"he has 150, including leather armor, and he tries to equip it, the remaining value is divided by 2, so 25kg)
  public string Name;
  public int Strength;
  public int Agility;
  public int MaxHP;
  public Inventory Inventory;
  public int Gold;
  public int Silver;
  public int Copper;

  public Weapon equippedWeapon;
  public Shield equippedShield;
  private Armor equippedArmor;

  //character init
  public Character(string name){
    Name = name;
    Strength = new Random().Next(1, 11);
    Agility = new Random().Next(1, 6); //negative, the less the better
    MaxHP = new Random().Next(50, 101);
    Inventory = new Inventory();
    if(Agility>Strength)
      Agility=Strength;
  }
  
  //yup, could have placed this directly in MaxHP inicialization, but it's supposed to be as OOP as possible:)
  public void CalculateMaxHP(){
    MaxHP = MaxHP + (Agility * 2);
  }

  public void addMoney(int gold, int silver, int copper){
    if(gold*0.05+silver*0.03+copper*0.01>Inventory.GetRemainingCapacity(Strength, Agility, countMoneyWeight(), equippedWeight())){
      Console.WriteLine($"Cant add {gold} gold, {silver} silver and {copper} copper, too heavy!");
    }else{
      Gold+=gold;
      Silver+=silver;
      Copper+=copper;
      Console.WriteLine($"Added {gold} gold, {silver} silver and {copper} copper."+Inventory.GetRemainingCapacity(Strength, Agility, countMoneyWeight(), equippedWeight()));
    }
  }

  public void EquipWeapon(Weapon weapon, Shield shield){
    Console.WriteLine($"{Name} is euipping {weapon.Name}...");
    if(Inventory.contains(weapon)){
      if(weapon.Weight <= Inventory.GetRemainingCapacity(Strength, Agility, countMoneyWeight(), equippedWeight())){
        if(weapon.Hands == 2 && equippedShield != null){
          Console.WriteLine($"{Name} cannot equip the two-handed weapon {weapon.Name} while having a {shield.Name} equipped.");
        }else{
          if(equippedWeapon!=null){
            Console.WriteLine("First de-equip your current weapon!");
          }else{
            equippedWeapon = weapon;
            Inventory.RemoveItem(weapon);
            Console.WriteLine($"{Name} equipped {weapon.Name}");
          }
        }
      }else{
        Console.WriteLine($"{Name} tried equipping {weapon.Name}, but it's too heavy, he wouldn't be able to swing it properly");
      }
    }else{
      Console.WriteLine($"{Name} cannot equip {weapon.Name} because it is not in the inventory.");
    }
  }

  public void EquipShield(Shield shield, Weapon weapon){
    Console.WriteLine($"{Name} is euipping {shield.Name}...");
    if(Inventory.contains(shield)){
      if(shield.Weight <= Inventory.GetRemainingCapacity(Strength, Agility, countMoneyWeight(), equippedWeight())){
        if(weapon.Hands == 2)
          Console.WriteLine($"{Name} cannot equip {shield.Name}, while wielding double-handed {weapon.Name}.");
        else{
          equippedShield = shield;
          Inventory.RemoveItem(shield);
          Console.WriteLine($"{Name} equipped {shield.Name}");
        }
      }else
        Console.WriteLine($"{Name} tried equipping {shield.Name}, but it's too heavy, he'd be dragging it on the ground");
    }else
      Console.WriteLine($"{Name} cannot equip {shield.Name} because it is not in the inventory.");
  }

  public void EquipArmor(Armor armor){
    Console.WriteLine($"{Name} is euipping {armor.Name}...");
    if(Inventory.contains(armor)){
      if(armor.Weight <= Inventory.GetRemainingCapacity(Strength, Agility, countMoneyWeight(), equippedWeight())){
        equippedArmor = armor;
        Inventory.RemoveItem(armor);
        Console.WriteLine($"{Name} equipped {armor.Name}");
      }else
        Console.WriteLine($"{Name} tried equipping {armor.Name}, but it's too heavy, he would snap his spine :(");
    }else
      Console.WriteLine($"{Name} cannot equip {armor.Name} because it is not in the inventory.");
  }

  public void UnequipWeapon(){
    if(equippedWeapon != null){
      Inventory.AddItem(equippedWeapon, this);
      Console.WriteLine($"{Name} unequipped {equippedWeapon.Name}");
      equippedWeapon = null;
    }else
      Console.WriteLine($"{Name} has no weapon equipped to unequip.");
  }

  public void UnequipShield(){
    if(equippedShield != null){
      Inventory.AddItem(equippedShield, this);
      Console.WriteLine($"{Name} unequipped {equippedShield.Name}");
      equippedShield = null;
    }else
      Console.WriteLine($"{Name} has no shield equipped to unequip.");
  }

  public void UnequipArmor(){
    if(equippedArmor != null){
      Inventory.AddItem(equippedArmor, this);
      Console.WriteLine($"{Name} unequipped {equippedArmor.Name}");
      equippedArmor = null;
    }else
      Console.WriteLine($"{Name} has no armor equipped to unequip.");
  }

  public void Attack(Character target){
    if(equippedWeapon != null){
      int attackValue = new Random().Next(1, 21) + Strength + equippedWeapon.Attack - Agility;
      Console.WriteLine($"{Name} attacks {target.Name} with the incredible force of {attackValue}!");
      target.Defend(attackValue);
    }else{
      int attackValue = new Random().Next(1, 21) + Strength - Agility;
      Console.WriteLine($"{Name} has no weapon to attack, yet he bravely attacks {target.Name} with his bare fists, producing unbelievable power of {attackValue}");
      target.Defend(attackValue);
    }
  }

  public void Defend(int attackValue){
    int defenseValue = Strength-Agility;
    if(equippedArmor != null)
      defenseValue += equippedArmor.Defense;
    if(equippedShield != null)
      defenseValue += equippedShield.Defense;
    int damageTaken = Math.Max(0, attackValue - defenseValue);
    MaxHP -= damageTaken;
    if(damageTaken==0)
      Console.WriteLine($"{Name} defends with power of {defenseValue}, and easily deflects the attack\n");
    else
      Console.WriteLine($"{Name} defends with power of {defenseValue}, yet takes {damageTaken} damage, leaving him with {MaxHP} HP remaining\n");
  }

  public void PrintInventory(){
    Console.WriteLine($"\n{Name}'s Inventory:");
    Inventory.PrintItems();
    if(equippedShield!=null ||equippedWeapon!=null ||equippedArmor!=null){
      Console.WriteLine("  Equipped:");
      if(equippedShield!=null)
        Console.WriteLine("    "+equippedShield.Name);
      if(equippedWeapon!=null)
        Console.WriteLine("    "+equippedWeapon.Name);
      if(equippedArmor!=null)
        Console.WriteLine("    "+equippedArmor.Name);
    }
    if(Gold!=0)
      Console.WriteLine($"  Gold Coin x{Gold}");
    if(Silver!=0)
      Console.WriteLine($"  Silver Coin x{Silver}");
    if(Copper!=0)
      Console.WriteLine($"  Copper Coin x{Copper}");
    Console.WriteLine($"  Money in coppers: {Copper+Silver*10+Gold*100}\n");
  }
  
  public double equippedWeight(){
    double totalWeight = 0;
    if(equippedArmor != null)
      totalWeight += equippedArmor.Weight;
    if(equippedShield != null)
      totalWeight += equippedShield.Weight;
    if(equippedWeapon != null)
      totalWeight += equippedWeapon.Weight;
    return totalWeight*2;
  }
  
  public double countMoneyWeight(){
      return Gold*0.05+Silver*0.03+Copper*0.01;
  }
}

class Item{
  public string Name;
  public double Weight;
  public Item(string name, double weight){
    Name = name;
    Weight = weight;
  }
}

class Weapon : Item{
  public int Attack;
  public int Hands;
  public Weapon(string name, double weight, int attack, int hands) : base(name, weight){
    Attack = attack;
    Hands = hands;
  }
}

class Shield : Item{
  public int Defense;
  public Shield(string name, double weight, int defense) : base(name, weight){
    Defense = defense;
  }
}

class Armor : Item{
  public int Defense;
  public Armor(string name, double weight, int defense) : base(name, weight){
    Defense = defense;
  }
}

class Inventory{
  private List<Item> items;
  public Inventory(){
    items = new List<Item>();
  }
  public void AddItem(Item item, Character player){
    Console.Write($"{player.Name} is adding {item.Name} to his inventory...");
    if(item.Weight<=player.Inventory.GetRemainingCapacity(player.Strength, player.Agility, player.countMoneyWeight(), player.equippedWeight())){
      items.Add(item);
      Console.Write(" Succes\n");
    }else
      Console.WriteLine("\nNope, thats too heavy");
  }
  public void RemoveItem(Item item){
    items.Remove(item);
  }
  public bool contains(Item item){
    return items.Contains(item);
  }  
  public double GetRemainingCapacity(int characterStrength, int characterAgility, double money, double equippedWeight){
    double totalWeight = money;
    foreach (Item item in items){
      totalWeight += item.Weight;
    }
    return characterStrength*20 - characterAgility - totalWeight - equippedWeight;
  }
  public void PrintItems(){
    foreach (Item item in items){
      Console.WriteLine($"  {item.Name}: {item.Weight}kg");
    }
  }
}