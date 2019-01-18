import styled from 'vue-styled-components';

const brandProps = { color: String, textColor: String };

// Create a <BrandButton> Vue component that renders a <button> which is
// the color of the passed in color
const BrandButton = styled('button', brandProps)`
  background-color: ${props => props.color} !important;
  color:  ${props => props.textColor} !important;
  border-radius: 0.3125rem;
  height: 2.4rem;
  font-size: 0.75rem;
  min-width: 8.75rem;
  margin-right: 0.5rem;

  &:disabled {
    background-color: #909090;
  }
`;

export default BrandButton;
