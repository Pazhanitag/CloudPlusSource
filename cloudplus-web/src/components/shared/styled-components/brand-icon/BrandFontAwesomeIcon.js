import styled from 'vue-styled-components';

const componentProps = {
  color: String,
};

const BrandFontAwesomeIcon = styled('i', componentProps)`
    color: ${props => props.color};
`;

export default BrandFontAwesomeIcon;
