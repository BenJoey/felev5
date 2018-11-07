#include <fstream>
#include <vector>
#include <sstream>
#include <string>
#include <future>

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
      stream<<"There it is! The dominant combination is:"<<std::endl;
      stream<<"Level: "<<Level<<std::endl;
      stream<<"Class: "<<Class<<std::endl;
      stream<<"Race: "<<Race<<std::endl;
      stream<<"Gender: "<<Gender<<std::endl;
    }
};

bool is_this_the_GOD_build(const std::vector<Build> Arr, const int buildnum, const int begin, const int end){
  if(buildnum == -1) return false;
  int count = 0;
  int ReqCount = (end - begin)/2;
  for(int i = begin; i< end; ++i)
    if(Arr[buildnum] == Arr[i]) count++;
  return (count > ReqCount);
}

int find_the_best_motha_fin_build(const std::vector<Build> Arr, const int begin, const int end){
  if(begin == (end-1)) return begin;

  int mid = (begin+end) / 2;

  std::future<int> leftResult = std::async(std::launch::async, find_the_best_motha_fin_build, Arr, begin, mid);
  std::future<int> rightResult = std::async(std::launch::async, find_the_best_motha_fin_build, Arr, mid, end);

  leftResult.wait();
  int leftDom = leftResult.get();

  if(is_this_the_GOD_build(Arr, leftDom, begin, end))
    return leftDom;

  rightResult.wait();
  int rightDom = rightResult.get();

  if(is_this_the_GOD_build(Arr, rightDom, begin, end))
    return rightDom;
  return -1;
}

int main()
{
  std::ifstream input("input.txt");
  std::vector<Build> data;
  std::string s;
  getline(input, s);
  unsigned int N = std::stoi(s);
  while(getline(input, s)){
    std::istringstream ss(s);
    std::vector<std::string> Line;
    while(getline(ss, s, ';')){
      Line.push_back(s);
    }
    Build temp(Line[0], Line[1], std::stoi(Line[2]), Line[3]);
    data.push_back(temp);
  }
  int DomBuild = find_the_best_motha_fin_build(data, 0, N);
  std::ofstream output("output.txt");
  if(DomBuild == -1){
    output << "We've found nothing interesting." << std::endl;
  }
  else{
    data[DomBuild].print(output);
  }
  output.close();
}
  //int count = 0, lsize = (end - begin)/2;
  /*if(leftDom != -1){
    for(int i = begin; i < end; ++i){
      if(Arr[leftDom] == Arr[i])count++;
    }
    std::cout<<count<<std::endl;
    if(count > lsize) return leftDom;
  }
  count = 0;
  rightResult.wait();
  int rightDom = rightResult.get();
  if(rightDom != -1){
    for(int i = begin; i < end; ++i){
      if(Arr[rightDom] == Arr[i])count++;
    }
    std::cout<<count<<std::endl;
    if(count > lsize) return rightDom;
  }
  return -1;*/
