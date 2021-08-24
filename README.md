# Biome Generator(By Heat Moisture Height)

 Biome Generator System! Made with Unity 2019.4.12f1

 Heat, Moisture, Height affect to biome Generating

 Generate biom by heat, moisture, height's value.
 
 Randomize all value with Perlin Noise.

 ## How to use?
 
 ### 1.
 
 <img src = "https://user-images.githubusercontent.com/49996889/130565340-ae409f77-973f-46e1-9ce8-7f309c0086c6.png" width="500">
 
 Select "Prefab" folder's Quad prefab.
 
 ### 2.
 
 <img src = "https://user-images.githubusercontent.com/49996889/130565946-47389890-c067-4fdd-9cd5-f904e8461237.png" width="500">

 |        | hottest |	hot |	cold |	coldest |
 | ------ | ------ | ------ | ------ | ------ |
 | dryest |	desert |	grassland |	tundra |	tundra |
 | dry |	savanna |	savanna |	boreal | forest |	tundra |
 | wet	| tropical | rainforest	| boreal | forest	| boreal | forest	| tundra |
 | wettest |	tropical | rainforest |	tropical | rainforest |	tundra |	tundra |

 I referred to this table. and I made two dimension array of biome data. If you want change biome, change these datas!
 
 ### 3.
 
 <img src = "https://user-images.githubusercontent.com/49996889/130570394-f79fb7a8-2c59-4eb0-a4ff-554c50708bb1.png" width="500">
 
 You can control randomize data by change waves value.

 ### 4.
 
 <img src = "https://user-images.githubusercontent.com/49996889/130570794-6a75f77d-e8ad-48a1-b42b-487e6745c0ba.png" width="500">

 This properties are data of per viewer's. (Height map, Moisture map, Heat map)
 
 ### 5.
 
 <img src = "https://user-images.githubusercontent.com/49996889/130571512-8df0e6fe-d9f1-4dce-9b32-c0588bd46938.png" width="500">
 
 You can pick visualization mode.

 ### 6.
 
 <img src = "https://user-images.githubusercontent.com/49996889/130571917-47b2b72c-0780-4d44-b1b3-2acc3d4806e8.png" width="500">
 
  You can control tree generation by change values.

 ### 7.
 
 <img src = "https://user-images.githubusercontent.com/49996889/130572170-519db5c8-10ef-4dd5-9dab-3936ebc83c21.png" width="500">
 
  And then system will auto generate trees by biome datas.

 
 ## Example
 
 ### HeatMap
 
 <img src = "https://user-images.githubusercontent.com/49996889/130562563-0cfb3326-91c8-45c9-bf46-078872303b04.jpg" width="500">
 
 
 ### HeightMap
 <img src = "https://user-images.githubusercontent.com/49996889/130562569-b86243ac-c7c3-4856-9aa3-a9510e819c91.jpg" width="500">


 ### MoistureMap
 <img src = "https://user-images.githubusercontent.com/49996889/130562571-ea057be2-db99-4ee5-88ed-fdc282501425.jpg" width="500">


 ### WholeMap
 <img src = "https://user-images.githubusercontent.com/49996889/130562572-1b21ee69-4a59-4279-95a4-f414225dcd6f.jpg" width="500">


 ## Credits
 
 Wise
 
 hmh9534@gmail.com
 
 https://github.com/PassingG/
 
 ## License
 
 MIT © Wise
