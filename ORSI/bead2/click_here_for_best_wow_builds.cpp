#include <fstream>
#include <vector>
#include <sstream>
#include <string>
#include <future>

//Hello everyone and welcome to my channel
//In today's video i will show you how can you find the best build in
//the awesome game called World of Warcraft

//First you have to play thousands of hours of WoW to find out which buids are even viable

class Build { //Now check what are the important details of a build
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

//Then we have to test these builds to find out which is the best to make other players uninstall the game
int find_the_best_motha_fkin_build(const std::vector<Build> Arr, const int begin, const int end){
  if(begin == (end-1)) return begin;

  //We start the testing by splitting up the builds into separate groups until all of the group has 1 build
  int mid = (begin+end) / 2;

  //Now we create a blizzard account for each of the builds to be able playtest them simultaneously on different servers
  //Note: this part requires a lot of money because builds usually go further than lvl 20
  std::future<int> leftResult = std::async(std::launch::async, find_the_best_motha_fkin_build, Arr, begin, mid);

  //If there is a best build in each group that smashes the noobs on the correspondig servers then we compare the group winners until we reach the full build list again
  int rightDom = find_the_best_motha_fkin_build(Arr, mid, end);

  if(is_this_the_GOD_build(Arr, rightDom, begin, end))
    return rightDom;

  leftResult.wait();
  int leftDom = leftResult.get();

  if(is_this_the_GOD_build(Arr, leftDom, begin, end))
    return leftDom;

  //If this part was not successful then we wasted the money for testing these builds
  return -1;
}

//Now that we know how to find the best build we have to execute it
int main()
{
  std::ifstream input("input.txt"); //Open the list of builds
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
    Build temp(Line[0], Line[1], std::stoi(Line[2]), Line[3]); //Only keep the important parts of the builds
    data.push_back(temp);
  }
  int DomBuild=-1;
  if(N!=0)DomBuild = find_the_best_motha_fkin_build(data, 0, N); //Run the process said earlier
  std::ofstream output("output.txt");
  if(DomBuild == -1){
    //If we have failed to find the best build because we are failures in life then all there is left to us
    //is to embrace the crippling depression and try to recover the money lost by making
    //heavy clickbait videos for YouTube that has the shit edited out of them

    output << "We've found nothing interesting." << std::endl;
  }
  else{
    data[DomBuild].print(output);
  }
  output.close();
}
//Thank you for watching guys
//If you enjoyed the video don't forget to like, comment, subscribe, share it on fb, post about it on instagram and twitter and also visit my patreon
