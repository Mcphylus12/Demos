import React from "react";
import { Link } from "react-router-dom";

interface NavbarProps
{
    links: NavBarLink[];
    right?: boolean
}

export interface NavBarLink
{
    url: string;
    display:string;
}

function Navbar(props: NavbarProps) {

    let links = props.links.map((l, index) => 
        <div className="items-stretch" key={index}>
            <Link to={l.url}>
                <span className="btn btn-ghost btn-sm rounded-btn">
                    {l.display}
                </span>
            </Link>
        </div>
    );

    const wrapper = {
        
    }

    return (
        <div className="navbar shadow-lg bg-neutral text-neutral-content">
            <div className="flex-1 px-2 mx-2">
                {!props.right && 
                    links
                }
            </div>
            <div className="flex-none px-2 mx-2">
                {props.right && 
                    links
                }
            </div>
        </div>
    );
}

export default Navbar;