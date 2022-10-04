import React, { useState, useEffect, useRef } from "react";

import socketIOClient from "socket.io-client";
import Carousels from "./components/Carousel";
import "bootstrap/dist/css/bootstrap.min.css";
import "./App.css";
import $ from 'jquery'; 
import { DisplayScreen } from "./components/Helpers";

function App() {
  const [data, setData] = useState([]);
  const [shelfTitle, setShelfTitle] = useState("");
  const [screen, setScreen] = useState(DisplayScreen.INIT);
  const [videoUrl, setVideoUrl] = useState("");
  const [noMessage, setNoMessage] = useState(false);
  const socketRef = useRef();
  var rfidRef = useRef();
  var timeOut, hideTimeOut;

  useEffect(() => {
    socketRef.current = socketIOClient.connect(process.env.REACT_APP_SERVER_SOCKET_HOST);
    socketRef.current.on("displayScreen", (res) => {
      if((res.type != "CM" && res.type != "DIGITAL_SIGNAGE") || res.rfid != rfidRef.current) {
        rfidRef.current = res.rfid;
        setData([]);
        setVideoUrl('');
        setScreen(DisplayScreen[res.type]);
        setNoMessage(false);
        //data empty, then show message
        if(res.data?.length == 0) {
          clearTimeout(timeOut);
          setNoMessage(true);
          timeOut = setTimeout(() => {
            setNoMessage(false);
            setScreen(DisplayScreen.INIT);
          }, 3000);
        } else {
          setTimeout(() => {
            setVideoUrl(res.videoUrl);
            setShelfTitle(res.title);
            setData(res.data);
            clearTimeout(timeOut);
            timeOut = setTimeout(() => {
              if(res.type == "CM"){
                setScreen(DisplayScreen.DIGITAL_SIGNAGE);
                clearTimeout(timeOut);
                timeOut = setTimeout(() => {
                  setScreen(DisplayScreen.INIT);
                  setData([]);
                }, process.env.REACT_APP_DISPLAY_SCREEN_TIMEOUT);
              } else {
                setScreen(DisplayScreen.INIT);
                setData([]);
              }
            }, res.type == "CM" ? 18000 : process.env.REACT_APP_DISPLAY_SCREEN_TIMEOUT);
          }, 1000);
        }
      }
    });

    window.addEventListener("load", () => {
      $(this).scrollTop(0);
      hideTimeOut = setTimeout(() => {
        $(".se-pre-con").hide();
      }, 4000);
    });

    window.addEventListener("beforeunload", () => {
      window.scrollTo(0, 0);
    });
    
    return () => {
      socketRef.current?.disconnect();
      rfidRef.current?.disconnect();
      clearTimeout(timeOut);
      clearTimeout(hideTimeOut);
    };
  }, []);

  return (
    <div>
      <Carousels screen={screen} title={shelfTitle} data={data} videoUrl={videoUrl} noMessage={noMessage}/>
    </div>
  );
}

export default App;
