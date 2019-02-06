#include <iostream>
#include <string>

class CityOne
{
    private:
  std::string population;
    int density;
    std::string based;
    std::string name;
    int area;
    int resultX;

    void SetPopulation(std::string _p)
    {
        population = _p;
    }

    void SetBased(std::string _b)
    {
        based = _b;
    }

    void SetName(std::string _n)
    {
        name = _n;
    }

    void SetArea(int _a)
    {
        area = _a;
    }

    void SetDensity(int _d)
    {
        density = _d;
    }
    int GetArea()
    {
        return area;
    }

    friend std::ostream& operator << (std::ostream& os, CityOne c)
    {
        os << "Название города/поселка : " << c.name << "   |||  Население : " << c.population << "   |||   Мер : " << c.based << "   |||   Плотность(чел/км.кв) : "
          << c.density << "   |||   Площадь(км.кв) : " << c.area;
        return os;
    }



    public:
  void City(std::string _p, std::string _b, std::string _n, int _a, int _d)
    {
        SetPopulation(_p);
        SetBased(_b);
        SetName(_n);
        SetArea(_a);
        SetDensity(_d);
    }

    void Sum(int _a, int _d)
    {
        resultX = area * density;
    }
};


class Town : public CityOne {
  const int based = 1653;
public:
  int GetBased()
{
    return based;
}
};

class Village : public CityOne {
  const int based = 1150;
public:
  int GetBased()
{
    return based;
}
};

class Kiev1 : public CityOne {
  const int based = 854;
public:
  int GetBased()
{
    return based;
}
};



int main()
{
    setlocale(LC_ALL, "Russian");
    Town Kharkiv;
    Kharkiv.City("1.000.000.000", " Стрелок ", "Харьков", 350, 4400);
    std::cout << Kharkiv;
    std::cout << std::endl << " Основан : " << Kharkiv.GetBased();

    std::cout << std::endl;

    Kiev1 Kiev;
    Kiev.City("75.000.000.000", " Боксер ", "Киев", 785, 12300);
    std::cout << Kiev;
    std::cout << std::endl << " Основан : " << Kiev.GetBased();

    std::cout << std::endl;


    Village Komsomolske;
    Komsomolske.City("1.000.000", " Артур-ка ", "Комсомольске", 9540, 1500);
    std::cout << Komsomolske;
    std::cout << std::endl << " Основан : " << Komsomolske.GetBased();

    std::cout << std::endl;
    system("pause");
    return 0;

}
int main()
{
    int All = 0;
    setlocale(LC_ALL, "Russian");
    Town Kharkiv;
    Kharkiv.City("1.000.000.000", " Стрелок ", "Харьков", 350, 4400);
    std::cout << Kharkiv;
    std::cout << std::endl << " Основан : " << Kharkiv.GetBased();
    All += Kharkiv.GetArea();
    std::cout << std::endl;

    Kiev1 Kiev;
    Kiev.City("75.000.000.000", " Боксер ", "Киев", 785, 12300);
    std::cout << Kiev;
    std::cout << std::endl << " Основан : " << Kiev.GetBased();
    All += Kiev.GetArea();
    std::cout << std::endl;


    Village Komsomolske;
    Komsomolske.City("1.000.000", " Артур-ка ", "Комсомольске", 9540, 1500);
    std::cout << Komsomolske;
    std::cout << std::endl << " Основан : " << Komsomolske.GetBased();
    All += Komsomolske.GetArea();
    std::cout << std::endl;

    std::cout << All << std::endl;
    system("pause");
    return 0;

}