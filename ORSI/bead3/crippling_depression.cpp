#include <string>
#include <fstream>
#include <vector>
#include <sstream>
#include "pipe.hpp"
#include <iostream>

class Candidate{
  public:
    std::string _submitdate, _job, _email, _picfname, _cvfname;
    std::vector<std::string> _skills;
    int _picsize, _cvsize;
    bool _isValid;
    //public:
    Candidate(std::string Line){
      std::istringstream ss(Line);
      std::vector<std::string> t;
      std::string s;
      for(int i = 1; getline(ss, s, '|'); ++i){
        if(i != 4) t.push_back(s);
        else{
          std::istringstream iss(s);
          std::string temp;
          while(getline(iss, temp, ';')) _skills.push_back(temp);
        }
      }
      _submitdate = t[0]; _email = t[1]; _job = t[2];
      _cvfname = t[3]; _cvsize = std::stoi(t[4]);
      _picfname = t[5]; _picsize = std::stoi(t[6]);
      _isValid = true;
    }
    void print(std::ostream& stream) const {
      stream << "date:"<<_submitdate<<std::endl;
      stream << "email:"<<_email<<std::endl;
      stream << "job:"<<_job<<std::endl;
      stream << "2nd skill:"<<_skills[1]<<std::endl;
      stream << "cvname:"<<_cvfname<<std::endl;
      stream << "cvsize:"<<_cvsize<<std::endl;
      stream << "picname:"<<_picfname<<std::endl;
      stream << "cvsize:"<<_picsize<<std::endl;
    }
};

std::ostream& operator<<(std::ostream& stream,const Candidate& rhs){
  rhs.print(stream);
  return stream;
}

int main()
{
  std::ifstream input("data.txt");
  std::string s;
  getline(input, s);
  std::cout<< s << std::endl;
  while(getline(input, s)){
    std::cout<< s << std::endl;
    Candidate test(s);
    std::cout << test;
  }
  return 0;
}
