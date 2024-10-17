using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

interface IKemampuan
{
    void  Gunakan(Robot target);
    bool IsCooldown();
    void ResetCooldown();

}

abstract class Robot
{
    public string Nama { get; protected set; }
    public int Energi { get; protected set; }
    public int Armor { get; private set; }
    public int Serangan { get; protected set; }

    public Robot(string nama, int energi, int armor, int serangan)
    {
        Nama = nama;
        Energi = energi;
        Armor = armor;
        Serangan = serangan;
    }

    public abstract void Serang(Robot target);
    public abstract void GunakanKemampuan(IKemampuan kemampuan, Robot target);
    public void CetakInformasi()
    {
        Console.WriteLine($"Nama: {Nama}, Energi: {Energi}, Armor: {Armor}, Serangan: {Serangan}");
    }

    public void KurangiEnergi(int jumlah)
    {
        Energi -= jumlah;
        if (Energi < 0) Energi = 0;
    }

    public void PulihkanEnergi(int jumlah)
    {
        Energi += jumlah;
    }

    public void TambahArmor(int jumlah)
    {
        Armor += jumlah;
    }

    public bool IsMati()
    {
        return Energi <= 0;
    }
}

class RobotBiasa : Robot
{
    public RobotBiasa(string nama, int energi, int armor, int serangan)
        : base(nama, energi, armor, serangan) { }

    public override void Serang(Robot target)
    {
        Console.WriteLine($"{Nama} menyerang {target.Nama}");
        int damage = Serangan - target.Armor;
        if (damage > 0)
        {
            target.KurangiEnergi(damage);

            Console.WriteLine($"{target.Nama} kehilangan {damage} energi");
        }
        else
        {
            Console.WriteLine($"{target.Nama} tidak terkena serangan");
        }
    }

    public override void GunakanKemampuan(IKemampuan kemampuan, Robot target)
    {
        if (!kemampuan.IsCooldown())
        {
            kemampuan.Gunakan(target);
            kemampuan.ResetCooldown();
        }
        else
        {
            Console.WriteLine("Kemampuan sedang cooldown");
        }
    }
}

class BosRobot : Robot
{
    private int pertahanan;

    public BosRobot(string nama, int energi, int armor, int serangan, int pertahanan)
        : base(nama, energi, armor, serangan)
    {
        this.pertahanan = pertahanan;
    }

    public override void Serang(Robot target)
    {
        Console.WriteLine($"{Nama} (BOS) menyerang {target.Nama}");
        int damage = Serangan - target.Armor;
        if (damage > 0)
        {
            target.KurangiEnergi(damage);
            Console.WriteLine($"{target.Nama} kehilangan {damage} energi"); 
        }
        else
        {
            Console.WriteLine($"{target.Nama} tidak terkena serangan");
        }
    }

    public void Diserang(Robot penyerang)
    {
        Console.WriteLine($"{Nama} diserang oleh {penyerang.Nama}");
        int damage = penyerang.Serangan - (Armor + pertahanan);
        if (damage > 0)
        {
            KurangiEnergi(damage);
            Console.WriteLine($"{Nama} (BOS) kehilangan {damage} energi ");
        }
        else
        {
            Console.WriteLine($"{Nama} (BOS) tidak terkena serangan");
        }
    }

    public void Mati()
    {
        if (IsMati())
        {
            Console.WriteLine($"{Nama} (BOS) telah dikalahkan!");
        }
    }

    public override void GunakanKemampuan(IKemampuan kemampuan, Robot target)
    {
        if (!kemampuan.IsCooldown())
        {
            kemampuan.Gunakan(target);
            kemampuan.ResetCooldown();
        }
        else
        {
            Console.WriteLine("Kemampuan sedang cooldown");
        }
    }
}

//implementasi beberapa kemampuan
class Perbaikan : IKemampuan
{
    private bool cooldown = false;

    public void Gunakan(Robot target)
    {
        Console.WriteLine($"Menggunakan kemampuan perbaikan pada {target.Nama}");
        target.PulihkanEnergi(20);
        Console.WriteLine($"{target.Nama} memulihkan 20 energi");
        cooldown = true;
    }

    public bool IsCooldown()
    {
        return cooldown;
    }

    public void ResetCooldown()
    {
        cooldown = false;
    }
}

class SeranganListrik : IKemampuan
{
    private bool cooldown = false;

    public void Gunakan(Robot target)
    {
        Console.WriteLine($"Menggunakan serangan listrik pada {target.Nama}");
        target.KurangiEnergi(30);
        Console.WriteLine($"{target.Nama} kehilangan 30 energi karena serangan listrik");
        cooldown = true;
    }

    public bool IsCooldown()
    {
        return cooldown;
    }

    public void ResetCooldown()
    {
        cooldown= false;
    }
}

class SeranganPlasma : IKemampuan
{
    private bool cooldown = false;

    public void Gunakan(Robot target)
    {
        Console.WriteLine($"Menggunakan serangan plasma pada {target.Nama}");
        target.KurangiEnergi(40);
        Console.WriteLine($"{target.Nama} kehilangan 40 energi karena terkena serangan plasma");
        cooldown = true;
    }

    public bool IsCooldown()
    {
        return cooldown;
    }

    public void ResetCooldown()
    {
        cooldown = false;
    }
}

class PertahananSuper : IKemampuan
{
    private bool cooldown = false;

    public void Gunakan(Robot target)
    {
        Console.WriteLine($"Menggunakan Pertahanan Super pada {target.Nama}");
        target.TambahArmor(20); //menggunakan metode tambaharmor untuk menambah armor
        Console.WriteLine($"{target.Nama} armor bertambah menjadi {target.Armor}");
        cooldown = true;
    }

    public bool IsCooldown()
    {
        return cooldown;
    }

    public void ResetCooldown()
    {
        cooldown= false;
    }
}

// main class
class program
{
    static void Main(string[] args)
    {
        RobotBiasa robot1 = new RobotBiasa("Robot A", 100, 10, 20);
        BosRobot boss = new BosRobot("Bos X", 200, 20, 30, 10);

        Perbaikan repair = new Perbaikan();
        SeranganListrik electricShock = new SeranganListrik();
        SeranganPlasma plasmaCannon = new SeranganPlasma();
        PertahananSuper superShield = new PertahananSuper();

        //menampilkan informasi robot
        robot1.CetakInformasi();
        Console.WriteLine();
        boss.CetakInformasi();
        Console.WriteLine() ;

        //serangan dan penggunaan kemampuan
        robot1.Serang(boss);
        Console.WriteLine();
        boss.Diserang(robot1);
        boss.Mati();
        Console.WriteLine();

        robot1.GunakanKemampuan(plasmaCannon, boss);
        boss.Mati();
        Console.WriteLine();

        //menggunakan kemampuan tambahan
        robot1.GunakanKemampuan(superShield, robot1);
        Console.WriteLine();
        boss.Mati();
    }
}
