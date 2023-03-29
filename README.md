# Unit Test

單元測試又稱為 AAA 測試，因此也可稱為 3A 測試，這三個 A 分別代表的英文為：

  * Arrange : 安排你的測試情境
  * Act : 呼叫你要的測試方法
  * Assert : 呼叫完要測試得方法後，你期望的結果跟你的實際結果是否符合

## 使用的函式庫
  * NUnit：單元測試框架
  * NSubstitute：做假資料隔離外部資源用的，因為單元測試不測試有讀檔、資料庫、網路連線及其他讀取外部資源的地方測試，因此，必須要對這些程式碼做隔離。

# 目的

  * 保護程式碼。假設今天修復了某段程式的 BUG，另一邊卻出現了 BUG，這時如果程式碼有完整單元測試保護的話，或許你可以馬上知道改了 A 卻壞了 B；但假設今天沒有單元測試的話，可能就不知道我改壞了程式碼。

# 測試的標的

  * 要測試的標的通常是程式碼的 public method, property, constructor，以及甚至是 internal method。
  * 測試的情境要包含：
  	* 正向測試
  	* 反向測試
    
# 測試規範
  * 測試的類別名稱，後面一律加上 **Tests** 這個字 (因為你可能會有很多條測試)，並在測試類別補上 TestFixture 之 Attribute。
  * 依照 AAA 規則進行命名中間用底線隔開，方法內容盡量以 AAA 規則進行分行。假設我有一個計算機裡面有加法運算並且要寫測試，因此，範例如下：
  ```c#
  [TestFixture]
  public class CalculactorTests
  {
      [Test]
      public void Add_WhenTwoNumbersAdd_ReturnSumOfTwoNumber() //測試標的_測試情境_期望結果
      {
          var calc = new Calculactor();
          var x = 10;
          var y = 20;

          var actualResult = calc.Add(x, y);

          Assert.That(actualResult, Is.EqualTo(30));
      }
  }  
  ```
  
  * 前面敘述有提到單元測試是不對有讀檔、資料庫、網路連線及其他讀取外部資源的地方測試，遇到這些請嘗試 Refactor，使用介面將這些程式碼抽象化。
  但注意不能破壞程式碼規格。例如：
  
  > 原本方法沒參數，經過重構後多了參數。
  
  **未重構程式碼**
  
  ```c#
    public class VideoService
    {
        public string ReadVideoTitle()
        {
            var str = File.ReadAllText("video.txt");
            var video = JsonConvert.DeserializeObject<Video>(str);
            if (video == null)
              return "Error parsing the video.";
            return video.Title;
        }
    }

    public class Video
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsProcessed { get; set; }
    }
  ```
  
  **重構後** 
  
  ```c#
    public class VideoService
    {
        public string ReadVideoTitle(IFileReader reader=null)
        {
            reader ??= new FileReader();
            
            var str = reader.ReadFile("video.txt");
            var video = JsonConvert.DeserializeObject<Video>(str);
          
            if (video == null)
              return "Error parsing the video.";
          
            return video.Title;
        }
    }

    public class Video
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsProcessed { get; set; }
    }

    public interface IFileReader 
    {
        string ReadFile(string path);
    }

    public class FileReader : IFileReader
    {
        public string ReadFile(string path)
        {
            return File.ReadAllText(path);
        }
    }
  ```
  
  # Unit Test Ninja
  
  * 這是練習單元測試的 kata，有興趣的同仁可以從這 github clone 下來練習。
  * 裡面有分 Fundamentals, Mocking, Difficulty
  
  ## 說明
  
  * Fundamentals：不需要使用到假物件的測試。
  * Mocking：可能需要使用到假物件，並可能要做點重構才能測試。
  * Difficulty：這我自己加上的 XDD。
  
  ## 傳送門
  * [unit test ninja](https://github.com/WindDYTING/UnitTestNinja.git)
  
  # References
  
  * [NUnit](https://docs.nunit.org/articles/nunit/writing-tests/assertions/assertion-models/constraint.html)
  * [NSubstitute](https://nsubstitute.github.io/help/return-for-any-args/)
