import { useState } from "react";
import Categories from "../Categories/Categories";
import { Product } from "../Products/Product";
import { useGetProductsQuery } from "../Products/ProductApi";
import ProductCard from "../Components/ProductCard";
import { useCreateOrderMutation } from "../Orders/OrderApi";

export default function Home() {
    const [category, setCategory] = useState<string | undefined>('');
    const { data } = useGetProductsQuery(category);
    const [createOrder] = useCreateOrderMutation();

    function onOrder(sum: Product): void {
        if (!sum.id)
        {
            throw new Error('Must specify a product');
        }

        createOrder({
            DeliveryInformation: "KEKW",
            Product: sum.id,
            Amount: 1
        });
    }

    const listItems = data?.map((sum, i) =>
        <div className="w-1/3 inline-block p-4" key={sum.id}>
            <ProductCard product={sum}  onOrder={() => onOrder(sum)}/>
        </div>
    );

    return (
        <div className="w-full flex flex-row items-stretch">
            <div className="flex-none w-1/5 mb-4">
                <Categories onChange={id => setCategory(id)}/>
            </div>
            <div className="grow">
                <ul className="m-4">
                    {data && listItems}
                </ul>
            </div>
        </div>
    );
}
