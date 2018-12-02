#include <string>
#include <thread>
#include <fstream>
#include <vector>
#include <algorithm> //for std::find
#include <sstream>
#include <tuple> //for std::tie
#include "pipe.hpp"

//Splits a string by the delimeter into the given vector
void LineToVec(const std::string Line, const char delimeter, std::vector<std::string>& ToFill){
  std::istringstream ss(Line);
  std::string s;
  while(getline(ss, s, delimeter)) ToFill.push_back(s);
}

class Candidate{
  public:
    std::string _submitdate, _job, _email, _picfname, _cvfname, _skills;
    int _picsize, _cvsize;
    bool Valid;
    Candidate(const std::string Line){
      std::vector<std::string> t;
      LineToVec(Line, '|', t); //Split the line into a vector by the '|' delimeter
      //Now use the vector to fill up the candidate's data
      _submitdate = t[0]; _email = t[1]; _job = t[2], _skills = t[3];
      _cvfname = t[4]; _cvsize = std::stoi(t[5]);
      _picfname = t[6]; _picsize = std::stoi(t[7]);
      Valid = true;
    }
};

//Pipe functions

void DateCompare(Pipe<Candidate>& source, Pipe<Candidate>& dest, const int data_count, const std::string FilterLine){
  std::vector<std::string> temp;
  LineToVec(FilterLine, ' ', temp);
  int year=std::stoi(temp[0]), month=std::stoi(temp[1]), day=std::stoi(temp[2]);
  auto deadline = std::tie(year, month, day);
  for(int i = 0; i < data_count; ++i){
    Candidate curr = source.pop();
    temp.clear(); LineToVec(curr._submitdate, ' ', temp);
    int subm_year=std::stoi(temp[0]), subm_month=std::stoi(temp[1]), subm_day=std::stoi(temp[2]);
    auto submdate = std::tie(subm_year, subm_month, subm_day);
    if(submdate > deadline) curr.Valid=false;
    dest.push(curr);
  }
}

//Email check and job availability search are almost identical so we so we differ them by addig a variable that tells which one is used
//value is 0 for email and 1 for job 
void EmailAndJob(Pipe<Candidate>& source, Pipe<Candidate>& dest, const int data_count, const std::string FilterLine, const int JobOrEmail){
  std::vector<std::string> ValidValues;
  LineToVec(FilterLine, '|', ValidValues);
  for(int i=0;i<data_count;++i){
    Candidate curr = source.pop();
    if(curr.Valid){
      std::string ToCheck = JobOrEmail == 0 ? curr._email.substr(curr._email.find("@")+1) : curr._job;
      if(std::find(ValidValues.begin(), ValidValues.end(), ToCheck) == ValidValues.end()) curr.Valid=false;
    }
    dest.push(curr);
  }
}

void SkillCheck(Pipe<Candidate>& source, Pipe<Candidate>& dest, const int data_count, const std::string FilterLine){
  std::vector<std::string> reqSkills;
  LineToVec(FilterLine, '|', reqSkills);
  for(int i=0;i<data_count;++i){
    Candidate curr = source.pop();
    if(curr.Valid){
      int count = 0;
      std::vector<std::string> cand_skills;
      LineToVec(curr._skills, ';', cand_skills);
      for(std::string curr_skill : cand_skills)
        if(std::find(reqSkills.begin(), reqSkills.end(), curr_skill) != reqSkills.end()) count++;
      if(count <= reqSkills.size()/2) curr.Valid=false;
    }
    dest.push(curr);
  }
}

void CVformat(Pipe<Candidate>& source, Pipe<Candidate>& dest, const int data_count){
  for(int i=0;i<data_count;++i){
    Candidate curr = source.pop();
    if(curr.Valid){
      //Get the extension of the CV and convert it to uppercase to be easier to check
      std::string usr_ext = curr._cvfname.substr(curr._cvfname.find_last_of("."));
      std::transform(usr_ext.begin(), usr_ext.end(), usr_ext.begin(), ::toupper);
      if(usr_ext != ".PDF") curr.Valid=false;
    }
    dest.push(curr);
  }
}

//The two size checks are also almost identical so we differ them by addig a variable that tells if this is for Pic or CV
//value is 0 for CV & 1 for Pic
void Size_Check(Pipe<Candidate>& source, Pipe<Candidate>& dest, const int data_count, const int MaxSize, const int PicOrCV){
  for(int i=0;i<data_count;++i){
    Candidate curr = source.pop();
    int ToCheck = PicOrCV == 0 ? curr._cvsize : curr._picsize;
    if(curr.Valid && ToCheck > MaxSize) curr.Valid=false;
    dest.push(curr);
  }
}

void Picformat(Pipe<Candidate>& source, Pipe<Candidate>& dest, const int data_count, const std::string FilterLine){
  std::vector<std::string> formats;
  LineToVec(FilterLine, '|', formats);
  for(int i=0;i<data_count;++i){
    Candidate curr = source.pop();
    if(curr.Valid && std::find(formats.begin(), formats.end(), curr._picfname.substr(curr._picfname.find_last_of("."))) == formats.end()) curr.Valid=false;
    dest.push(curr);
  }
}

void FinalPipe(Pipe<Candidate>& source, const int data_count){
  std::ofstream output("output.txt");
  for(int i = 0;i<data_count;++i){
    Candidate curr = source.pop();
    if(curr.Valid) output << curr._email << std::endl;
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
}
