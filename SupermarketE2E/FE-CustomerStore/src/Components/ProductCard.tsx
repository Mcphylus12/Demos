import { Product } from "../Products/Product";

interface Props {
    product: Product;
    onOrder?: () => void;
}

export default function ProductCard(props: Props) {
    const num = Math.floor(Math.random() * 1005);

    return (
        <div className="card text-center hover:shadow-2xl transition-shadow">
            <figure className="px-10 pt-10">
                <img src={`https://picsum.photos/id/${num}/400/250`} className="rounded-xl" />
            </figure> 
            <div className="card-body">
                <h2 className="card-title">{props.product.name}</h2> 
                <p>{props.product.description}</p> 
                <div className="justify-center card-actions">
                <button className="btn btn-outline btn-accent" onClick={props.onOrder}>Order</button>
                </div>
            </div>
        </div> 
    );
}