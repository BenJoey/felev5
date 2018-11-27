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

//Email check and job availability search are almost identical so we so we differ them by addig a variable that tells which one is used
//value is 0 for email and 1 for job 
void EmailAndJob(Pipe<Candidate>& source, Pipe<Candidate>& dest, int data_count, std::string FilterLine, int JobOrEmail){
  std::vector<std::string> ValidValues;
  LineToVec(FilterLine, '|', ValidValues);
  for(int i=0;i<data_count;++i){
    Candidate curr = source.pop();
    if(curr.Valid()){
      std::string ToCheck = JobOrEmail == 0 ? curr._email.substr(curr._email.find("@")+1) : curr._job;
      if(std::find(ValidValues.begin(), ValidValues.end(), ToCheck) == ValidValues.end()) curr.Disable();
    }
    dest.push(curr);
  }
}
/*
void isJobAvailable(Pipe<Candidate>& source, Pipe<Candidate>& dest, int data_count, std::string FilterLine){
  std::vector<std::string> jobs;
  LineToVec(FilterLine, '|', jobs);
  for(int i=0;i<data_count;++i){
    Candidate curr = source.pop();
    if(curr.Valid() && std::find(jobs.begin(), jobs.end(), curr._job)==jobs.end()) curr.Disable();
    dest.push(curr);
  }
}*/

void SkillCheck(Pipe<Candidate>& source, Pipe<Candidate>& dest, int data_count, std::string FilterLine){
  std::vector<std::string> reqSkills;
  LineToVec(FilterLine, '|', reqSkills);
  for(int i=0;i<data_count;++i){
    Candidate curr = source.pop();
    if(curr.Valid()){
      int count = 0;
      for(std::string curr_skill : curr._skills)
        if(std::find(reqSkills.begin(), reqSkills.end(), curr_skill) != reqSkills.end()) count++;
      if(count <= reqSkills.size()/2) curr.Disable();
    }
    dest.push(curr);
  }
}

void CVformat(Pipe<Candidate>& source, Pipe<Candidate>& dest, int data_count){
  for(int i=0;i<data_count;++i){
    Candidate curr = source.pop();
    if(curr.Valid() && curr._cvfname.substr(curr._cvfname.size()-4) != ".pdf") curr.Disable();
    dest.push(curr);
  }
}

//The two size checks are also almost identical so we differ them by addig a variable that tells if this is for Pic or CV
//value is 0 for CV & 1 for Pic
void Size_Check(Pipe<Candidate>& source, Pipe<Candidate>& dest, int data_count, int MaxSize, int PicOrCV){
  for(int i=0;i<data_count;++i){
    Candidate curr = source.pop();
    int ToCheck = PicOrCV == 0 ? curr._cvsize : curr._picsize;
    if(curr.Valid() && ToCheck > MaxSize) curr.Disable();
    dest.push(curr);
  }
}

void Picformat(Pipe<Candidate>& source, Pipe<Candidate>& dest, int data_count, std::string FilterLine){
  std::vector<std::string> formats;
  LineToVec(FilterLine, '|', formats);
  for(int i=0;i<data_count;++i){
    Candidate curr = source.pop();
    if(curr.Valid() && std::find(formats.begin(), formats.end(), curr._picfname.substr(curr._picfname.find_last_of("."))) == formats.end()) curr.Disable();
    dest.push(curr);
  }
}

void FinalPipe(Pipe<Candidate>& source, int data_count){
  std::ofstream output("output.txt");
  for(int i = 0;i<data_count;++i){
    Candidate curr = source.pop();
    if(curr.Valid()) output << curr._email << std::endl;
  }
  output.close();
}

int main()
{
  std::vector<Pipe<Candidate>> pipes(9);
  std::ifstream input("data.txt");
  std::ifstream filters("filters.txt");
  std::string s;
  getline(input, s);
  int N = std::stoi(s);
  getline(filters, s); std::thread t1(DateCompare, std::ref(pipes[0]), std::ref(pipes[1]), N, s);
  getline(filters, s); std::thread t2(EmailAndJob, std::ref(pipes[1]), std::ref(pipes[2]), N, s, 0);
  getline(filters, s); std::thread t3(EmailAndJob, std::ref(pipes[2]), std::ref(pipes[3]), N, s, 1);
  getline(filters, s); std::thread t4(SkillCheck, std::ref(pipes[3]), std::ref(pipes[4]), N, s);
  std::thread t5(CVformat, std::ref(pipes[4]), std::ref(pipes[5]), N);
  getline(filters, s); std::thread t6(Size_Check, std::ref(pipes[5]), std::ref(pipes[6]), N, std::stoi(s), 0);
  getline(filters, s); std::thread t7(Picformat, std::ref(pipes[6]), std::ref(pipes[7]), N, s);
  getline(filters, s); std::thread t8(Size_Check, std::ref(pipes[7]), std::ref(pipes[8]), N, std::stoi(s), 1);
  std::thread t9(FinalPipe, std::ref(pipes[8]), N);
  filters.close();
  while(getline(input, s)){
    Candidate test(s);
    pipes[0].push(test);
  }
  input.close();
  t1.join();t2.join();t3.join();t4.join();
  t5.join();t6.join();t7.join();t8.join();t9.join();
  return 0;
}
