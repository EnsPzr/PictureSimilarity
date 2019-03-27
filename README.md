<h2>Picture Similarity Resim Karşılaştırma</h2>
<q>Eng:</q>
<b>This library has been created for a simple comparison of multiple image files. </b>
There are two different methods.
The first method takes the path of the main picture as a parameter, the paths of the other images and the minimum likeness ratio.
Returns the path of the similar ones in the List <string> type.
The use of this method is shown in Figure 1.
The second method takes the path of the main picture as a parameter and the paths of other images.
Back to the top The dictionary <string, returns a percentage of how similar the main type to the parent image is.
The use of this method is shown in Figure 2.
The system works as follows:
<ul>
  <li>All images are taken to the size of the main picture.</li>  
  <li>Pictures are all converted to gray and then black and white.</li>
  <li>Pixels are compared with other pictures by main picture.</li>
</ul>
<h3><b><i>NOTE: As the color changes to black and white, the success rate is low.</i></b></h3>
<q>Tr:</q>
<b>Bu kütüphane birden fazla resim dosyasının basit bir şekilde karşılaştırılması için oluşturulmuştur.</b>
İki farklı metot vardır.
Birinci metot parametre olarak ana resmin yolunu, diğer resimlerin yollarını ve minimum benzerlik oranını alır.
Geriye benzerlerin yolunu List<string> tipinde döndürür.
Bu metodun kullanımı resim 1 de gösterilmiştir.
İkinci metot ise parametre olarak ana resmin yolunu ve diğer resimlerin yollarını alır.
Geriye dictionary<string,double> türünde hangi resmin ana resme ne kadar benzer olduğunu yüzde olarak döndürür.
Bu metodun kullanımı resim 2 de gösterilmiştir.
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
