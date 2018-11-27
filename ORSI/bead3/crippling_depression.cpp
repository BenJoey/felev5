#include <string>
#include <thread>
#include <fstream>
#include <vector>
#include <algorithm>
#include <sstream>
#include <tuple>
#include "pipe.hpp"
#include <iostream>

void LineToVec(std::string Line, char separator, std::vector<std::string>& ToFill){
  std::istringstream ss(Line);
  std::string s;
  while(getline(ss, s, separator)) ToFill.push_back(s);
}

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
          LineToVec(s, ';', _skills);
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
  auto deadline = std::tie(year, month, day);
  for(int i = 0; i < data_count; ++i){
    Candidate curr = source.pop();
    int subm_year, subm_month, subm_day;
    std::istringstream buffer2(curr._submitdate);
    if(buffer2 >> subm_year) buffer.ignore();
    if(buffer2 >> subm_month) buffer.ignore();
    buffer2 >> subm_day;
    auto submdate = std::tie(subm_year, subm_month, subm_day);
    if(submdate > deadline) curr.Disable();
    dest.push(curr);
  }
}

void EmailCheck(Pipe<Candidate>& source, Pipe<Candidate>& dest, int data_count, std::string FilterLine){
  std::vector<std::string> validmails;
  LineToVec(FilterLine, '|', validmails);
  for(int i=0;i<data_count;++i){
    Candidate curr = source.pop();
    if(curr.Valid()){
      std::string maildomain = curr._email.substr(curr._email.find("@")+1);
      if(std::find(validmails.begin(), validmails.end(), maildomain) == validmails.end()) curr.Disable();
    }
    dest.push(curr);
  }
}

void isJobAvailable(Pipe<Candidate>& source, Pipe<Candidate>& dest, int data_count, std::string FilterLine){
  std::vector<std::string> jobs;
  LineToVec(FilterLine, '|', jobs);
  for(int i=0;i<data_count;++i){
    Candidate curr = source.pop();
    if(curr.Valid() && std::find(jobs.begin(), jobs.end(), curr._job)==jobs.end()) curr.Disable();
    dest.push(curr);
  }
}

void SkillCheck(Pipe<Candidate>& source, Pipe<Candidate>& dest, int data_count, std::string FilterLine){
  std::vector<std::string> reqSkills;
  LineToVec(FilterLine, '|', reqSkills);
  for(int i=0;i<data_count;++i){
    Candidate curr = source.pop();
    if(curr.Valid()){
      int count = 0;
      for(std::string curr_skill : curr._skills)
        count += std::find(reqSkills.begin(), reqSkills.end(), curr_skill) == reqSkills.end() ? 1 : 0;
      if(count <= reqSkills.size()/2) curr.Disable();
    }
    dest.push(curr);
  }
}

void FinalPipe(Pipe<Candidate>& source, int data_count){
  std::ofstream output("output.txt");
  for(int i = 0;i<data_count;++i){
    Candidate curr = source.pop();
    std::cout << curr._email << std::endl;
    if(curr.Valid()) output << curr._email << std::endl;
  }
  output.close();
}

int main()
{
  std::vector<Pipe<Candidate>> pipes(4);
  std::ifstream input("data.txt");
  std::string s;
  getline(input, s);
  std::thread t1(DateCompare, std::ref(pipes[0]), std::ref(pipes[1]), 3, "22 10 07");
  std::thread t2(EmailCheck, std::ref(pipes[1]), std::ref(pipes[2]), 3, "gmail.com|yahoo.com|hotmail.com");
  std::thread t3(isJobAvailable, std::ref(pipes[2]), std::ref(pipes[3]), 3, "3D Animator|Technical Lead");
  std::thread t9(FinalPipe, std::ref(pipes[3]), 3);
  while(getline(input, s)){
    Candidate test(s);
    pipes[0].push(test);
  }
  return 0;
}
