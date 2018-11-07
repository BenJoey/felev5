#ifndef BUILD_H
#define BUILD_H

#include <string>
#include <iostream>

class Build {
    private:
    int Level;
    std::string Race;
    std::string Class;
    std::string Gender;
    public:
    Build() {}
    Build(std::string Cla, std::string Rac,int Lev, std::string Gen) : Level(Lev), Class(Cla), Race(Rac), Gender(Gen) {}
    bool operator==(const Build& rhs) const{
        return Level == rhs.Level && Class == rhs.Class && Race == rhs.Race && Gender == rhs.Gender;
    }
    void print(std::ostream& stream) const{
        stream<<Level<<";"<<Class<<";"<<Race<<";"<<Gender<<std::endl;
    }
};
std::ostream& operator<<(std::ostream& stream,const Build& rhs){
    rhs.print(stream);
    return stream;
}

#endif //BUILD_H