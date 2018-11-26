#include <vector>
#include <sstream>
#include "pipe.hpp"

class Candidate{
  private:
    std::string _submitdate, _job, _email, _picfname, _cvfname;
    std::vector<std::string> _skills;
    int _picsize, _cvsize;
    bool _isValid;
  public:
    Candidate(std::string Line){
      std::istringstream ss(Line);
      int i = 1;
      std::string s;
      while(getline(ss, s, '|')){
        switch(i){
          case 1: _submitdate = s;break;
          case 2: _email = s;break;
          case 3: _job = s;break;
          case 5: _cvfname = s;break;
          case 6: _cvsize = std::stoi(s);break;
          case 7: _picfname = s;break;
          case 8: _picsize = std::stoi(s);break;
          default:break;
        }
        ++i;
      }
};

int main()
{
  return 0;
}
