function App() {
    return <CommentBox />;
}

function CommentBox() {
    const [name, SetName] = React.useState("");
    const [lastName, SetLastName] = React.useState("");

    function Submit(event) {
        event.preventDefault();

        const data = new FormData();
        data.append('ClientFirstName', name);
        data.append('ClientLastName', lastName);

        const xhr = new XMLHttpRequest();
        xhr.open('post', "/inquiries/new", true);
        xhr.onload = () => { };
        xhr.send(data);
        SetName("")
        SetLastName("")
    }

    return (
        <div>
            <div className="commentBox">Hi from React</div>
            <form className="commentForm" onSubmit={(e)=>Submit(e)}>
                <input type="text" placeholder="Your name"
                    value={name} onChange={(e) => SetName(e.target.value)} />   
                <label>{name}</label>
                <input type="text" placeholder="Your surname"
                    value={lastName} onChange={(e) => SetLastName(e.target.value)} /> 
                <label>{lastName}</label>
                <input type="submit" value="Post" />
            </form>
        </div>
    );
}

function Headline() {
    const greeting = 'Hello Function Component!';

    return <h1>{greeting}</h1>;
}

ReactDOM.render(<App />, document.getElementById('content'));