import styled from 'vue-styled-components';

const brandProps = { color: String, textColor: String };

// Create a <BrandUploadFileButton> Vue component that renders a <span> which is
// the color of the passed in color
const BrandUploadFileButton = styled('span', brandProps)`
  background-color: ${props => props.color} !important;
  color: ${props => props.textColor} !important;
  border-radius: 0.3125rem;
  height: 2.4rem;
  font-size: 0.75rem;
  min-width: 8.75rem;

  &:disabled {
    background-color: #909090;
  }

  &:focus {
    color: white;
  }

  &:hover {
    background-color: ${props => props.color};
  }
`;

export default BrandUploadFileButton;
