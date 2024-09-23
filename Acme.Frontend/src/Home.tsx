import {
  Box,
  Button,
  Card,
  CardBody,
  Container,
  Flex,
  Heading,
  Input,
  Modal,
  ModalBody,
  ModalCloseButton,
  ModalContent,
  ModalFooter,
  ModalHeader,
  ModalOverlay,
  SimpleGrid,
  Spinner,
  VStack,
  useDisclosure,
  useToast,
  Text,
} from "@chakra-ui/react";
import { ChangeEvent, useEffect, useState } from "react";
import { Subscriber } from "./types";
import moment from "moment";

const sortingOptions = [
  {
    title: "Sort by date (asc)",
    value: 0,
  },
  {
    title: "Sort by date (desc)",
    value: 1,
  },
];

const Home = () => {
  const [subscribers, setSubscribers] = useState<Subscriber[]>([]);
  const [filteredSubscribers, setFilteredSubscribers] = useState<Subscriber[]>(
    []
  );
  const [files, setFiles] = useState<File[]>([]);
  const { isOpen, onOpen, onClose } = useDisclosure();
  const sortingComponent = useDisclosure();
  const [isButtonLoading, setIsButtonLoading] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const [searchInput, setSearchInput] = useState("");
  const [sortOption, setSortOption] = useState(0);
  const toast = useToast();

  const onModalClose = () => {
    setFiles([]);
    onClose();
  };

  const onSearch = (event: ChangeEvent<HTMLInputElement>) => {
    const text = event.currentTarget.value;
    setSearchInput(text);
    const filteredData = subscribers.filter(
      (x) =>
        x.email.includes(text) ||
        moment(x.expirationDate).format("YYYY-MM-DD HH:MM:SS").includes(text)
    );
    sort(filteredData, sortOption);
  };

  const onSortChange = (option: number) => {
    setSortOption(option);
    sort(filteredSubscribers, option);
  };

  const sort = (subsribersToSort: Subscriber[], option: number) => {
    if (option === 1) {
      const sorted = subsribersToSort.sort(
        (a, b) =>
          new Date(b.expirationDate).getTime() -
          new Date(a.expirationDate).getTime()
      );
      setFilteredSubscribers([...sorted]);
    } else {
      const sorted = subsribersToSort.sort(
        (a, b) =>
          new Date(a.expirationDate).getTime() -
          new Date(b.expirationDate).getTime()
      );
      setFilteredSubscribers([...sorted]);
    }
  };

  async function fetchData() {
    const response = await fetch("https://localhost:7219/api/Subscriptions");
    if (response.ok) {
      const data = await response.json();
      setSubscribers(data);
      sort(data, sortOption);
    }
    setIsLoading(false);
  }

  useEffect(() => {
    fetchData();
  }, []);

  const onImport = async () => {
    const formData = new FormData();
    files.forEach((file) => {
      formData.append("files", file);
    });

    setIsButtonLoading(true);
    const response = await fetch(
      "https://localhost:7219/api/FileImport/import",
      {
        method: "POST",
        body: formData,
      }
    );

    if (response.ok) {
      // Handle successful response
      const data = await response.json();
      const expiredSubscriptions = data.expiredSubscriptions;
      toast({
        title: "Data imported",
        description:
          expiredSubscriptions.length > 0
            ? `Several subscriptions were expired: ${JSON.stringify(
                expiredSubscriptions
              )}`
            : "Data imported successfully.",
        status: "success",
        duration: 9000,
        isClosable: true,
      });
      console.log("Files imported successfully");
    } else {
      const data = await response.json();
      if (data.errors) {
        console.log(JSON.stringify(data.errors));
        toast({
          title: "Error while importing data",
          description: `${JSON.stringify(data.errors)}`,
          status: "error",
          duration: 9000,
          isClosable: true,
        });
      }
    }
    setIsButtonLoading(false);
    onClose();
    fetchData();
  };

  const onFilesChange = (event: ChangeEvent<HTMLInputElement>) => {
    setFiles(Array.from(event.currentTarget.files!));
  };

  if (isLoading) {
    return <Spinner />;
  }

  return (
    <>
      <Container m="auto" my={50} maxW="1200px">
        <Heading>Subscribers</Heading>
        <Flex justifyContent="space-between">
          <Button onClick={onOpen} mt={5}>
            Import file
          </Button>
          <Flex gap={5} mt={5}>
            <Input
              w={200}
              placeholder="Search..."
              value={searchInput}
              onChange={onSearch}
            />
            <Box position="relative" w={150}>
              <Button
                onClick={() => sortingComponent.onToggle()}
                width="100%"
                justifyContent="space-between"
                backgroundColor="gray.100"
                _hover={{ backgroundColor: "gray.200" }}
              >
                <Text fontWeight="bold">Sort</Text>
                {sortingComponent.isOpen ? "▲" : "▼"}
              </Button>
              <Box
                display={sortingComponent.isOpen ? "block" : "none"}
                position="absolute"
                top="100%" // Position just below the button
                left="0"
                width="100%"
                zIndex={10}
                backgroundColor="white"
                borderWidth="1px"
                borderRadius="md"
                boxShadow="md"
                marginTop="1" // Adds a little space between the button and the dropdown
              >
                <VStack align="start" padding={4}>
                  {sortingOptions.map((opt) => {
                    return (
                      <Text
                        cursor="pointer"
                        _hover={{
                          fontWeight: "bold",
                        }}
                        fontWeight={
                          sortOption === opt.value ? "bold" : "normal"
                        }
                        onClick={() => onSortChange(opt.value)}
                      >
                        {opt.title}
                      </Text>
                    );
                  })}
                </VStack>
              </Box>
            </Box>
          </Flex>
        </Flex>
        <SimpleGrid mt={5} gap={5} columns={3}>
          {filteredSubscribers.map((subscriber) => {
            return (
              <Card w={350}>
                <CardBody>
                  <Flex flexDir="column">
                    <Box>
                      <Box fontWeight="bold">Email:</Box>
                      <Box>{subscriber.email}</Box>
                    </Box>
                    <Box>
                      <Box fontWeight="bold">Expiration date:</Box>
                      <Box>
                        {moment(subscriber.expirationDate).format(
                          "YYYY-MM-DD HH:mm:SS"
                        )}
                      </Box>
                    </Box>
                  </Flex>
                </CardBody>
              </Card>
            );
          })}
        </SimpleGrid>
      </Container>
      <Modal isOpen={isOpen} onClose={onModalClose}>
        <ModalOverlay />
        <ModalContent>
          <ModalHeader>Import files</ModalHeader>
          <ModalCloseButton />
          <ModalBody>
            <Input type="file" multiple onChange={onFilesChange} />
            <Box>Selected files: </Box>
            <VStack spacing={1} align="start" mt={2}>
              {files.map((file) => (
                <Text key={file.name}>{file.name}</Text>
              ))}
            </VStack>
          </ModalBody>

          <ModalFooter>
            <Flex gap={5}>
              <Button
                isDisabled={files && files.length < 1}
                isLoading={isButtonLoading}
                colorScheme="green"
                onClick={onImport}
              >
                Import
              </Button>
              <Button onClick={onModalClose}>Close</Button>
            </Flex>
          </ModalFooter>
        </ModalContent>
      </Modal>
    </>
  );
};

export default Home;
