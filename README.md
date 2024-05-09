Mój generator tworzy teren za pomocą Perlinga. Łatwo można dodać do niego kolejne warstwy (ale trzeba w kodzie). W aktualnej wersji są 4. 

Oprócz tego rozrzucam losowe miejsca specjalne w losowe miejsca. W generatorze jest tylko jeden typ miejsca specjalnego, ale je również łatwo dodać (ale trzeba w kodzie).
Na koniec łączę wszystkie miejsca specjalne drogami z losowym innym miejscem specjalnym. Łączę miejsca specjalne za pomocą A*.

Czas tworzenia zależy od wielkości mapy i liczby specjalnych miejsc. Przy 20 miejscach specjalnych i mapie o wielkości 160x90 trwa u mnie nawet ponad dwie minuty, a dla mapy 50x50 jakieś 10 sekund. Wszystkie parametry do sterowania generatora są możliwe do zmienienia w edytorze w skrypcie SCR_MapGeneratorManager w obiekcie PFB_Map znajdującym się na scenie.

Wszystkie tekstury stworzyłem sam, a ponieważ to nie moja specjalność, to cieszę się, że oczy od nich nie bolą bardzo. Teren jest animowany tylko jeśli kamera jest dostatecznie przybliżona, żeby to zobaczyć i tylko w obszarze widzianym przez kamerę.

Sterowanie kamerą: wsad / strzałki + scroll myszy 
