#include <string>
#include <fstream>
#include <vector>
#include <sstream>
#include <tuple>
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
    void Disable() { _isValid = false;}
    bool Valid() {return _isValid;}
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

void DateCompare(Pipe<Candidate>& source, Pipe<Candidate>& dest, int data_count, std::string FilterLine){
  std::istringstream buffer(FilterLine);
  int year, month, day;
  if(buffer >> year) buffer.ignore();
  if(buffer >> month) buffer.ignore();
  buffer >> day;
  for(int i = 0; i < data_count; ++i){
    Candidate data = source.pop();
    int subm_year, subm_month, subm_day;
    if(buffer >> subm_year) buffer.ignore();
    if(buffer >> subm_month) buffer.ignore();
    buffer >> subm_day;
    auto date1 = std::tie(year, month, day);
    auto date2 = std::tie(subm_year, subm_month, subm_day);
  }
}

int main()
{
  std::vector<Pipe<Candidate>> pipes(9);
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
