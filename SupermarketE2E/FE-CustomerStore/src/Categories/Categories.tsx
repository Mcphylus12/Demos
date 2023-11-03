import { useEffect } from "react";
import { useGetCategoriesQuery } from "./CategoryApi";

interface Props {
    onChange: (id?: string) => void;
}

export default function Categories(props: Props)
{
    const { data } = useGetCategoriesQuery();

    let categories = data?.map((l, index) => 
        <li className="btn btn-ghost btn-block rounded-none" key={index} onClick={() => props.onChange(l.id)}>
            {l.name}
        </li>
    );

    useEffect(() => {
        data && data.length > 0 && props.onChange(data[0].id);
    }, [data]);

    return (
        <ul>
            {categories}
        </ul>
    );
}
