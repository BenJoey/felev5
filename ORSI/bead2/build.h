#ifndef BUILD_H
#define BUILD_H

#include <string>
#include <iostream>

class build {
    private:
    int Level;
    std::string Race;
    std::string Class;
    std::string Gender;
    public:
    build() {}
    build(std::string Cla, std::string Rac,int Lev, std::string Gen) : Level(Lev), Class(Cla), Race(Rac), Gender(Gen) {}
    // void set_level(const int& Lev){
        // Level = Lev;
    // }
    bool operator==(const build& rhs) const{
        return Level == rhs.Level && Class == rhs.Class && Race == rhs.Race && Gender == rhs.Gender;
    }
    void print(std::ostream& stream) const{
        stream<<Level<<";"<<Class<<";"<<Race<<";"<<Gender<<std::endl;
    }
};
std::ostream& operator<<(std::ostream& stream,const build& rhs){
    rhs.print(stream);
    return stream;
}

#endif //BUILD_H