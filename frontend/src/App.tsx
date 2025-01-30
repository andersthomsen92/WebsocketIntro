import { useState, useEffect } from "react";

const App = () => {
    const [messages, setMessages] = useState<string[]>([]);
    const [messageContent, setMessageContent] = useState<string>(""); 
    const [ws, setWs] = useState<WebSocket | null>(null); 

    useEffect(() => {
        const socket = new WebSocket("ws://localhost:8181");

        socket.onmessage = (message) => {
            setMessages((prevMessages) => [...prevMessages, message.data]);
        };

        setWs(socket);

        return () => {
            socket.close();  // Otherwise it sends the message 2 to 4 times
        };
    }, []);


    const sendMessage = () => {
            // @ts-ignore
        ws.send(messageContent);
            setMessageContent(""); 
    };

    return (
        <div>
            <ul>
                {messages.map((msg, index) => (
                    <li key={index}>{msg}</li>
                ))}
            </ul>

            <input
                type="text"
                value={messageContent}
                onChange={(e) => setMessageContent(e.target.value)}
                placeholder="Type a message"
            />
            <button onClick={sendMessage}>Send</button>
        </div>
    );
};

export default App;