---
title: "Alexandria: A Virtual Reality EPUB Reader for Oculus Quest 2"
author: "Gaurang Ruparelia"
email: "gr2159@nyu.edu"
year: 2023
---

## Abstract
Alexandria is a Virtual Reality (VR) application developed for the Oculus Quest 2, focusing on reinventing the experience of reading digital books in EPUB format. The project aims to create a unique reading environment that extends beyond the limitations of traditional digital platforms like PCs and mobile devices. Utilizing Unity, along with the VersOne.Epub library, HTML Agility Pack, Endless Book Unity plugin, and Simple File Browser Plugin, Alexandria addresses the technical challenges of EPUB parsing, realistic book rendering, and user-friendly navigation in a VR space. The application's design philosophy revolves around enhancing the reader's engagement with digital texts, providing an immersive library setting that combines the comfort of traditional reading with the interactivity and visual appeal of VR. Alexandria represents an exploration into how VR can transform and enrich the experience of engaging with literature.

## Introduction
The history of reading has continually evolved, from
etching on stones to scrolls of papyrus, from bound paper
to digital pixels. In this tradition, I present Alexandria, a
project designed to be a step forward in this progression,
aiming to recreate the reading experience in virtual reality (VR). The Oculus Quest 2, with its advanced features
and wide adoption, serves as the perfect platform for this
project.
Prior to Alexandria, reading books in VR was a compromised experience, typically achieved by accessing nonnative applications like Kindle through a web browser. The
result was often text that appeared less sharp and engaging
than what VR technology could potentially offer. Moreover, the rich immersive potential of VR suggested that a
native epub reader for this environment could significantly
enhance the reader’s engagement. This project was inspired
by the challenge to replicate the deep, engrossing experience of reading long-form content in VR.
The core objective of Alexandria has been to create a reading experience where the application recedes into the
background as the user becomes absorbed in the narrative.
Just as one might lose awareness of the act of turning pages
in a physical book, so too should the user of Alexandria
forget they are interacting with a virtual interface, becoming
completely engrossed in the story.
The foundation of this application is built upon the conviction that reading is inherently enjoyable, and it represents
the initial steps toward a new evolution of reading—one
where narratives captivate not just visually but through multisensory engagement, including haptic feedback, sound,
and immersive environments.

## Results and Demonstration
Alexandria successfully integrates an ancient virtual library environment where users can upload and read EPUB
books. Key features demonstrated include the file upload
interface, EPUB parsing, and realistic text rendering on a
virtual book with book animations akin to a real-life book.
The application’s user interface, designed within Unity, offers intuitive navigation and interaction, making reading
in VR as comfortable and engaging as reading in the real world.
The experience of reading the book is extremely important. A good reading experience is one where the reader is
lost in the author’s words and the book almost disappears
as they are transported into the author’s world. The whole
point of the reading experience is to establish a connection
between the author and reader and for the reading mechanism to disappear. A physical book achieves this and so do
devices like Kindle. I am pleased to report that Alexandria
achieves this immersive quality too, thanks to its familiar
book interface, complete with detailed page flip animations
and sounds.
From the user’s perspective, the appflow is straightforward: they grant permission to access their Quest 2 files,
upload a book, and then the book loads. The file upload interface is then hidden, allowing the user to start reading and
move around the environment.
Under the hood, the book is stored in a persistent Data
folder on Android. The EPUB book parsing and text rendering occur on a render texture.
This entire setup contributes to an accessible and easyto-navigate user experience, even for beginners.

## Implementation
The project was divided into several phases:
1. Development of a File Upload Interface using Unity Simple File Browser Plugin.
2. EPUB Parsing Mechanism with VersOne.Epub library and HTML Agility Pack.
3. Camera Render Texture and Page Formatting for dynamic text changes.
4. Book Control Logic using a charIndex array and pageIndex integer.
5. Integration of All Elements into a VR Library Environment, including a locomotion system and a snap turn system.

## Discussion
Feedback highlighted the importance of font and text size for an optimal reading experience. Future enhancements include cloud storage, a virtual shelf for multiple books, image rendering within books, and expanding device compatibility.

## Related Work
- Research on "User Experience of Reading in Virtual Reality" exploring optimal text parameters.
- VR Cool Reader, an extension of the CoolReader 3 platform for VR.
- VR Productivity Apps focusing on virtual desktops and environments.

## Conclusion
Alexandria joins the VR trend of reimagining successful software for immersive environments. It showcases VR's potential to make reading more engaging and interactive, while retaining the essence of traditional reading. Future enhancements promise to expand the application's functionality and user appeal.

### References
- Kojić, Tanja, et al. "User Experience of Reading in Virtual Reality — Finding Values for Text Distance, Size and Contrast." 2020 Twelfth International Conference on Quality of Multimedia Experience (QoMEX).
- EndlessBook. [Link](https://assetstore.unity.com/packages/3d/props/endlessbook-134213)
- EpubReader. [Link](https://os.vers.one/EpubReader/)
- VRCoolReader. [GitHub Repository](https://github.com/incshaun/VRCoolReader)
- Unity File Browser. [GitHub Repository](https://github.com/yasirkula/UnitySimpleFileBrowser)
- VR Desktop. [Website](https://www.vrdesktop.net/)
- Html Agility Pack. [Website](https://html-agility-pack.net/)

### Figures
#### Figure 1: A Book Being Read in Alexandria
![A book being read in Alexandria](book.png)

### Acknowledgements
Special thanks to those who have provided feedback and support throughout the development of Alexandria. Their insights have been invaluable in shaping the project's direction and enhancing its user experience.

### Author Information
Gaurang Ruparelia is a passionate developer with a focus on VR technology. His work on Alexandria reflects his commitment to pushing the boundaries of digital reading and exploring the potential of immersive experiences.

#### Contact Information
Email: gr2159@nyu.edu

### Conference Information
Paper ID: *****
Conference Name: CVPR
Conference Year: 2023

### Notes
- Additional improvements and enhancements are planned for future versions of Alexandria, including cloud storage integration and expanded device compatibility.
- The aim is to make Alexandria not just a reading tool, but a comprehensive digital library experience in VR.

---

#### End of Document
