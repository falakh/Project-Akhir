Untuk memasukan memanipulasi AI
1.buka scene GameFix di CoOpTankGame->Scenes->GameFix
2.Pilih tank player 1 atau 2, edit script AI_CONTROLLER.cs lalu non aktifkan script tsb di tank yg lain.
3.mulai game dari scene menu untuk memilih map pertama kali, jika sdah pernah milih map dari menu, maka bsa langsung coba dari GameFix untuk langsung ke permainan tanpa memilih map
4. untuk bisa AI vs AI anda wajib membuat script AI_Controller dengan nama baru untuk tank yg satunya agar bisa saling bertanding dengan kecerdasan yg berbeda, anda bisa copas dari Ai_Controller yg sdh ada dan anda sesuaikan sendiri 
5. cara penggunaan code bisa dibaca d script  AI_CONTROLLER.cs , contoh penggunaan behavior tree (tdk wajib) ada d script mathtree.cs (harus ikut kuliah behavtree d kelas dulu)

Note :
Error terjadi biasanya karena anda langsung play dari GameFix scene, 
Mulailah play pertama dari scene menu dan lalu pilihlah arena, jika sdh pernah melakukan ini maka baru anda bisa mulai langsung dari gameFix scene