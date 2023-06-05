import React, { Component } from "react";
import ParticlesBg from "particles-bg";

class Header extends Component {
    render() {
        return (
            <h1>
<ParticlesBg color="#ff0000" num={200} type="circle" bg={true} />
</h1>
        )
    }
}

export default Header;