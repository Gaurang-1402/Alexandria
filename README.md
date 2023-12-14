This was my final project submission for [CS-GY 9223: VIRTUAL AND AUGMENTED REALITY](https://nyu-icl.github.io/courses/2022fall-vr-ar/index.html#home).

## Video

#### Alexandria VR Application Demo
[![Alexandria VR Application Demo](https://github.com/Gaurang-1402/Alexandria/assets/71042887/5930b7f4-0465-48f9-a7f5-9c93dd46d8ce)](https://www.youtube.com/watch?v=gcQaYHthc9A)

Click the image or the link to watch the demonstration video of Alexandria, showcasing its features and user experience in action.

Link to the video: [Alexandria VR App Demo](https://www.youtube.com/watch?v=gcQaYHthc9A&t=19s)

## Introduction
The history of reading has continually evolved, from etching on stones to scrolls of papyrus, from bound paper to digital pixels. In this tradition, I present Alexandria, a project designed to be a step forward in this progression, aiming to recreate the reading experience in virtual reality (VR). The Oculus Quest 2, with its advanced features and wide adoption, serves as the perfect platform for this project. Prior to Alexandria, reading books in VR was a compromised experience, typically achieved by accessing non-native applications like Kindle through a web browser. The result was often text that appeared less sharp and engaging than what VR technology could potentially offer. Moreover, the rich immersive potential of VR suggested that a native epub reader for this environment could significantly enhance the reader’s engagement. This project was inspired by the challenge to replicate the deep, engrossing experience of reading long-form content in VR. The core objective of Alexandria has been to create a reading experience where the application recedes into the background as the user becomes absorbed in the narrative. Just as one might lose awareness of the act of turning pages in a physical book, so too should the user of Alexandria forget they are interacting with a virtual interface, becoming completely engrossed in the story.
The foundation of this application is built upon the conviction that reading is inherently enjoyable, and it represents the initial steps toward a new evolution of reading—one where narratives captivate not just visually but through multisensory engagement, including haptic feedback, sound, and immersive environments.


## Results and Demonstration
Alexandria successfully integrates an ancient virtual library environment where users can upload and read EPUB books. Key features demonstrated include the file upload interface, EPUB parsing, and realistic text rendering on a virtual book with book animations akin to a real-life book. The application’s user interface, designed within Unity, offers intuitive navigation and interaction, making reading in VR as comfortable and engaging as reading in the real world. The experience of reading the book is extremely important. A good reading experience is one where the reader is lost in the author’s words and the book almost disappears as they are transported into the author’s world. The whole point of the reading experience is to establish a connection between the author and reader and for the reading mechanism to disappear. A physical book achieves this and so do devices like Kindle. I am pleased to report that Alexandria achieves this immersive quality too, thanks to its familiar book interface, complete with detailed page flip animations and sounds.

From the user’s perspective, the appflow is straightforward: they grant permission to access their Quest 2 files, upload a book, and then the book loads. The file upload interface is then hidden, allowing the user to start reading and move around the environment. Under the hood, the book is stored in a persistent Data folder on Android. The EPUB book parsing and text rendering occur on a render texture. This entire setup contributes to an accessible and easy-to-navigate user experience, even for beginners.

![ezgif com-video-to-gif-converted](https://github.com/Gaurang-1402/Alexandria/assets/71042887/240432d4-2673-42f9-91aa-b6b012b5676c)


## Implementation
The project was structured into several key phases:
1. Development and Integration of a File Upload Interface:
I utilized the Unity Simple File Browser Plugin, which is
originally designed for Android devices, and adapted it for
VR compatibility.
2. EPUB Parsing Mechanism: EPUB books use a specific
HTML formatting. I extracted the raw HTML-formatted
text from the EPUB books using the VersOne.Epub library.
Then, I employed the HTML Agility Pack to isolate the
book text from its surrounding HTML tags. 
3. Camera Render Texture and Page Formatting: The text
on each page dynamically changes with every page flip,
both forward and backward. To achieve this, I created
camera render textures for the left and right pages. During
runtime, the text in these camera render texture pages is
dynamically updated. Through trial and error, I determined
the optimal number of words per page and adjusted the font
size for clear legibility.
4. Book Control Logic: This was the most enjoyable part
of the project for me. The key components of the book
control logic include a charIndex array, which tracks the
starting character on any given left or right page, and a
pageIndex integer that increments or decrements with each
page flip. For the book animations and materials, I utilized
the Endless Book plugin available on Unity.
5. Integration of All Elements into a VR Library Environment: The final step involved integrating all these
elements into a two-floor library environment, complete with bookshelves and a desk where the user’s main book
appears post-upload. I developed a locomotion system and
a snap turn system, enabling users to walk around and carry
the book with them if they wish.


## Discussion
Having received feedback from friends and family who
tested Alexandria, it became apparent that the font and text
size are crucial for an optimal reading experience. Therefore, a necessary improvement is to increase the book’s text
size.
Looking ahead, there are several enhancements I plan
to implement. One key feature is cloud storage for users’
uploaded books and their reading progress, enabling readers to seamlessly resume their reading from where they last
stopped. Another addition I aim to introduce is a virtual
shelf where users can store and access multiple books. A
significant future development would be the inclusion of
image rendering within the books, which might require the
creation of an HTML and CSS renderer. This is a sub-stantial task, explaining its absence in the current version.
Furthermore, expanding the range of supported file formats
and incorporating interactive features like note-taking could
greatly enhance user experience.
A notable limitation of Alexandria at present is its exclusive compatibility with the Oculus Quest 2. In future
versions, I plan to broaden the range of compatible devices
to increase accessibility for a wider user base.


## Conclusion
Alexandria joins the ongoing movement in virtual reality (VR), where successful software from the mobile and PC
domains are reimagined for immersive environments. This
project showcases how VR can be harnessed to transform
the traditional act of reading into a more engaging and interactive experience, without losing the essence of what makes
reading enjoyable. In using the Oculus Quest 2, Alexandria
leverages the platform’s capabilities to improve the user’s
reading experience, offering a blend of the familiar and the
new. The application’s commitment to replicating the comfort of reading a physical book is evident in its choice of
EPUB format, lifelike book animations, and an intuitive
user interface. Looking forward, the introduction of features like cloud storage and broader device compatibility,
along with other interactive elements, will undoubtedly expand the application’s appeal and functionality. Alexandria
is a testament to how digital reading is evolving, with VR
playing a significant role in shaping how we engage with
long-form narratives in the future.


### References
- Kojić, Tanja, et al. "User Experience of Reading in Virtual Reality — Finding Values for Text Distance, Size and Contrast." 2020 Twelfth International Conference on Quality of Multimedia Experience (QoMEX).
- EndlessBook. [Link](https://assetstore.unity.com/packages/3d/props/endlessbook-134213)
- EpubReader. [Link](https://os.vers.one/EpubReader/)
- VRCoolReader. [GitHub Repository](https://github.com/incshaun/VRCoolReader)
- Unity File Browser. [GitHub Repository](https://github.com/yasirkula/UnitySimpleFileBrowser)
- VR Desktop. [Website](https://www.vrdesktop.net/)
- Html Agility Pack. [Website](https://html-agility-pack.net/)


---

