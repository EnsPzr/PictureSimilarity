<h2>Picture Similarity Resim Karşılaştırma</h2>
<q>Eng:</q>
<b>This library has been created for a simple comparison of multiple image files. </b>
There are three different methods.
<h3>Method 1</h3>
The first method takes the path of the main picture as a parameter, the paths of the other images and the minimum likeness ratio.
Returns the path of the similar ones in the List <string> type.
The use of this method is shown in Figure 1.<br />
<h3>Method 2</h3>
The second method takes the path of the main picture as a parameter and the paths of other images.
Back to the top The dictionary <string, returns a percentage of how similar the main type to the parent image is.
The use of this method is shown in Figure 2.<br />
<h3>Method 3</h3>
The third method takes the paths of all the images to be compared as parameters.      
A model with Img1, Img2, and SimilarityRatio fields is returned.
The Img1 parameter contains the actual image based on the comparison.
The parameter Img2 contains the picture which is compared to the original based on the comparison.
SimilarityRatio returns the similarity ratio.
The use of this method is shown in Figure 3.<br />
The system works as follows:
<ul>
  <li>All images are taken to the size of the main picture.</li>  
  <li>Pictures are all converted to gray and then black and white.</li>
  <li>Pixels are compared with other pictures by main picture.</li>
</ul>
<h3><b><i>NOTE: As the color changes to black and white, the success rate is low.</i></b></h3>
<q>Tr:</q>
<b>Bu kütüphane birden fazla resim dosyasının basit bir şekilde karşılaştırılması için oluşturulmuştur.</b>
Üç farklı metot vardır.
<h3>Metot 1</h3>
Birinci metot parametre olarak ana resmin yolunu, diğer resimlerin yollarını ve minimum benzerlik oranını alır.
Geriye benzerlerin yolunu List<string> tipinde döndürür.
Bu metodun kullanımı resim 1 de gösterilmiştir.<br />
<h3>Metot 2</h3>
İkinci metot ise parametre olarak ana resmin yolunu ve diğer resimlerin yollarını alır.
Geriye dictionary<string,double> türünde hangi resmin ana resme ne kadar benzer olduğunu yüzde olarak döndürür.
Bu metodun kullanımı resim 2 de gösterilmiştir.<br />
<h3>Metot 3</h3>
Üçüncü metot parameter olarak karşılaştırılmak istenen tüm resimlerin yollarını almaktadır.
Geriye içerisinde Img1, Img2 ve SimilarityRatio alanları bulunan bir model döndürmektedir.
Img1 parametresi karşılaştırılmada asıl baz alınan resmi içermektedir.
Img2 parametresi karşılıştırılmada asıl baz alınan resim ile karşılaştırılan resmi içermektedir.
SimilarityRatio benzerlik oranını döndürmektedir.
 Bu metodun kullanımı resim 3 de gösterilmiştir.<br />
Sistem şu şekilde çalışır:
<ul>
  <li>Tüm resimler ana resmin boyutuna getirilir.</li>  
  <li>Resimlerin hepsi önce griye, sonra siyah beyaza dönüştürülür.</li>
  <li>Pikseller gezilerek ana resim ile diğer resimler karşılaştırılır.</li>
</ul>
<h3><b><i>NOTE: Siyah beyaza çevrilme yapıldığı için renkli resimlerde başarı oranı düşük olmaktadır.</i></b></h3>
  <h2> Image 1 </h2>
<img src="/anlatim1.png" />
  <h2> Image 2 </h2>
<img src="/anlatim2.png" />
    <h2> Image 3 </h2>
<img src="/anlatim3.png" />
